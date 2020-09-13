#!groovy
/* Plots a JMeter response time CSV input file to HTML with embedded PNG charts.
 * Generates:
 *  - Response times over time scatter plot
 *  - Response time percentiles
 *  - Transactions per minute over time
 *  - Aggregates: Response time averages, 90th percentiles, transaction count
 * Implemented as Unix filter with stdin/stdout streams.
 * CSV must have at least the columns (and a header specifying their order):
 *   timeStamp,label,elapsed
 * With timeStamp column format as below or epoch milliseconds.
 * Usage: cat [input-csv] | groovy [this-script] [[max-millis]] > [output-html]
 */
DTFORMAT = 'yyyy-MM-dd HH:mm:ss.SSS'
MAXMILLIS = ((args as List)[0] ?: 20000).toLong()
TPMINTERVAL = 60000 // 1 minute

//@Grab('org.knowm.xchart:xchart:3.2.2')
import static org.knowm.xchart.style.markers.SeriesMarkers.CIRCLE
import static org.knowm.xchart.style.markers.SeriesMarkers.NONE
import static org.knowm.xchart.XYSeries.XYSeriesRenderStyle.Scatter
import static org.knowm.xchart.XYSeries.XYSeriesRenderStyle.Line
import static org.knowm.xchart.BitmapEncoder.BitmapFormat.PNG
import static org.knowm.xchart.BitmapEncoder.getBitmapBytes

def data = System.in.readLines()*.split(',').with { 
    def headers = first()
    def timeStamp = headers.findIndexOf { it ==~ /timeStamp/ }
    def elapsed   = headers.findIndexOf { it ==~ /elapsed/ } 
    def label     = headers.findIndexOf { it ==~ /label/ } 
    def success   = headers.findIndexOf { it ==~ /success/ } 
    drop(1).collect { row -> 
        [timeStamp: row[timeStamp] ==~ /\d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d\.\d+/ ? new Date().parse(DTFORMAT, row[timeStamp]) : new Date(row[timeStamp].toLong()), 
        elapsed:    row[elapsed].toLong(), 
        label:      row[label],
        success:    success ? row[success] : null]
}   }

if (data.empty) throw new RuntimeException('No input data')

def startTime = (data.timeStamp*.getTime().min()/TPMINTERVAL).toLong()
def endTime = ((data.timeStamp*.getTime().max()+TPMINTERVAL)/TPMINTERVAL).toLong()
def timeRange = (0..<(endTime-startTime)).collectEntries { [((startTime+it)*TPMINTERVAL): 0] } // empty histogram for TPM calculations

def charts =  new org.knowm.xchart.XYChartBuilder().width(1200).height(600).with { [
	responsetimes : title("Response Times")           .xAxisTitle("Time")      .yAxisTitle("Milliseconds")           .build(),
	percentiles   : title("Percentile Response Times").xAxisTitle("Percentile").yAxisTitle("Milliseconds")           .build(),
	throughput    : title("Throughput")               .xAxisTitle("Time")      .yAxisTitle("Transactions per Minute").build(),
] }

def aggregates = [
	averages        : [:],
	percentiles90th : [:],
	transactions    : [:],
]

charts.responsetimes.styler
    .setDefaultSeriesRenderStyle(Scatter)
    .setMarkerSize(6)
	.setYAxisMax(MAXMILLIS)

charts.percentiles.styler
    .setDefaultSeriesRenderStyle(Line)
	.setYAxisMax(MAXMILLIS)

charts.throughput.styler
	.setDefaultSeriesRenderStyle(Line)
	.setMarkerSize(0)

data.groupBy { it.label }.each { label, series ->
    def timeStamp = series.timeStamp
    def elapsed = series.elapsed
	charts.responsetimes.addSeries(label, timeStamp, elapsed).setMarker(CIRCLE)
	
	def range = (0..99)*.multiply(series.size()/100.0)*.toLong()
	def percentiles = elapsed.toSorted()[range]
	charts.percentiles.addSeries(label, (0..99), percentiles).setMarker(NONE)
	
	def transactions = timeStamp
		.collect { it.getTime()-it.getTime()%TPMINTERVAL } // truncate to full minute
		.inject(timeRange.clone().withDefault { 0 }) { tpm, minute -> tpm[minute] += 1; tpm } // count numbers of requests for each minute
		.collectMany { minute, count -> [[t:new Date(minute), tpm:count]] } // separate into 2 columns
	charts.throughput.addSeries(label, transactions.t, transactions.tpm)
	
	aggregates.averages[label] = elapsed.with{sum()/size()}
	aggregates.percentiles90th[label] = percentiles[90]
	aggregates.transactions[label] = transactions.tpm.sum()
}

new groovy.xml.MarkupBuilder().html { 
	head { title("Response Times") } 
	body { table { 
		tr {
			td { img(src: "data:image/png;base64,${getBitmapBytes(charts.responsetimes, PNG).encodeBase64()}") }
			td { table { 
				tr { th("Request"); th("Average Response Times (milliseconds)") }
				aggregates.averages.collect { label, value -> tr { td(label); td(sprintf('%.0f', value)) } }
		}	}	}
		tr {
			td { img(src: "data:image/png;base64,${getBitmapBytes(charts.percentiles, PNG).encodeBase64()}") }
			td { table { 
				tr { th("Request"); th("90th Percentile Response Times (milliseconds)") }
				aggregates.percentiles90th.collect { label, value -> tr { td(label); td(value) } }
		}	}	}
		tr {
			td { img(src: "data:image/png;base64,${getBitmapBytes(charts.throughput, PNG).encodeBase64()}") }
			td { table { 
				tr { th("Request"); th("Transaction Count") }
				aggregates.transactions.collect { label, value -> tr { td(label); td(value) } }
}	}	}	}	}	}

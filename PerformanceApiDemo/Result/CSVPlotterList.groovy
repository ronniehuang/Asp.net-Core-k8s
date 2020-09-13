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

def aggregates = [
	averages        : [:],
	percentiles90th : [:],
	transactions    : [:],
	maxResponse		: [:],
	minResponse		: [:],
]

data.groupBy { it.label }.each { label, series ->
    def timeStamp = series.timeStamp
    def elapsed = series.elapsed
	
	
	def range = (0..99)*.multiply(series.size()/100.0)*.toLong()
	def percentiles = elapsed.toSorted()[range]
	def maxResponse = elapsed.max()
	def minResponse = elapsed.min()
	
	def transactions = timeStamp
		.collect { it.getTime()-it.getTime()%TPMINTERVAL } // truncate to full minute
		.inject(timeRange.clone().withDefault { 0 }) { tpm, minute -> tpm[minute] += 1; tpm } // count numbers of requests for each minute
		.collectMany { minute, count -> [[t:new Date(minute), tpm:count]] } // separate into 2 columns
	
	aggregates.averages[label] = elapsed.with{sum()/size()}
	aggregates.percentiles90th[label] = percentiles[90]
	aggregates.transactions[label] = transactions.tpm.sum()
	aggregates.maxResponse[label] = maxResponse
	aggregates.minResponse[label] = minResponse
}

println aggregates["averages"]["Get Auth"]

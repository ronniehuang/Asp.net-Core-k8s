/* Helper script/class that plots JMeter files to PNGs and generates an HTML file with embedded images.
* Needs PATH environment variable to contain $JMETER_HOME/bin or %JMETER_HOME%\bin.
* Usage examples:
* - new JMeterPlotter().input('rt_*.csv').output('ResponseTimes.html').run('--plugin-type ResponseTimesOverTime')
* - new JMeterPlotter(base: '../target', options: '--paint-markers yes --line-weight 0 --plugin-type ResponseTimesOverTime').with {
*      input('rt_1.csv').output('ResponseTimes1.html').run()
*      input('rt_2.csv').output('ResponseTimes2.html').run()
*   }
*/

class JMeterPlotter {
    String base = '.'         // directory for relative file search
    String input              // Ant-style filename glob pattern e.g. **/*.csv
    String output             // HTML output filename (optional, defaults to stdout)
    String options = ''       // Command line options as per https://jmeter-plugins.org/wiki/JMeterPluginsCMD/#Usage-and-Parameters
    int width = 1800, height = 1080 // Image size

    def input(String input)     { this.input   = input;   this }
    def options(String options) { this.options = options; this }
    def output(String output)   { this.output  = output;  this }
    def height(int height)      { this.height  = height;  this }
    def width(int width)        { this.width   = width;   this }

    private Map pngs             // filename to image map (internal, temp)
    //private String plotCommand = System.properties['os.name'].toLowerCase().contains('windows') ? 'JMeterPluginsCMD.bat' : 'JMeterPluginsCMD.sh'
	private String plotCommand = 'JMeterPluginsCMD.sh'
    def run() { run(options) }

    def run(String _options) {
		
        pngs = [:]
        new FileNameFinder().getFileNames(base, input).each {
            def png = File.createTempFile('jmeter', '.png')
            png.deleteOnExit()
			
            def cmd = [plotCommand, '--width', width, '--height', height, '--relative-times', 'no', '--paint-gradient', 'no', '--input-jtl', it, '--generate-png', png]
            cmd.addAll "$options ${_options}".split(/ +/)
            cmd.execute().with {
                consumeProcessOutput(System.err, System.err)
                waitFor()
            }
            pngs[it] = png
        }
        writeHtml(output)
    } 

    def writeHtml(output) {
        def out = output ? new File(base, output) : System.out
        new groovy.xml.MarkupBuilder(new PrintWriter(out)).html {
            body { table {
                pngs.collect { png ->
                    tr { img(src: "data:image/png;base64,${png.value.bytes.encodeBase64()}") }
        }   }   }   }
        this
    }
}

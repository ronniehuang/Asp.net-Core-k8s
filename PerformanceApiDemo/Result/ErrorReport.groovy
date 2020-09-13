#!groovy
import javax.xml.transform.stream.StreamSource
import javax.xml.transform.stream.StreamResult

javax.xml.transform.TransformerFactory.newInstance()
    .newTransformer(new StreamSource(new File("jmeter-results-detail-report_21.xsl"))).with {
    setParameter 'dateReport', new Date()
    setParameter 'showData', 'y'
    transform new StreamSource(System.in), new StreamResult(System.out)
}
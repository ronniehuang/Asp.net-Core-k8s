<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

<!--
   Licensed to the Apache Software Foundation (ASF) under one or more
   contributor license agreements.  See the NOTICE file distributed with
   this work for additional information regarding copyright ownership.
   The ASF licenses this file to You under the Apache License, Version 2.0
   (the "License"); you may not use this file except in compliance with
   the License.  You may obtain a copy of the License at
 
       http://www.apache.org/licenses/LICENSE-2.0
 
   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
-->

<!-- 
	Stylesheet for processing 2.1 output format test result files 
	To uses this directly in a browser, add the following to the JTL file as line 2:
	<?xml-stylesheet type="text/xsl" href="../extras/jmeter-results-detail-report_21.xsl"?>
	and you can then view the JTL in a browser
-->

<xsl:output method="html" indent="yes" encoding="UTF-8" doctype-public="-//W3C//DTD HTML 4.01 Transitional//EN" />

<!-- Defined parameters (overrideable) -->
<xsl:param    name="showData" select="'n'"/>
<xsl:param    name="titleReport" select="'Load Test Error Report'"/>
<xsl:param    name="dateReport" select="'date not defined'"/>

<!-- http://www.jenitennison.com/xslt/grouping/muenchian.xml -->
<xsl:key name="results-by-label" match="testResults/*" use="@lb" />

<xsl:template match="testResults">
	<html>
		<head>
			<title><xsl:value-of select="$titleReport" /></title>
			<style type="text/css">
				body {
					font:normal 68% verdana,arial,helvetica;
					color:#000000;
				}
                table {
                    table-layout:fixed; 
                }
				table tr td, table tr th {
					font-size: 68%;
                    padding-right:10px;
				}
				table.details tr th{
				    color: #ffffff;
					font-weight: bold;
					text-align:center;
					background:#2674a6;
					white-space: pre-wrap;
				}
				table.details tr td{
					background:#eeeee0;
					white-space: pre-wrap;
				}
				h1 {
					margin: 0px 0px 5px; font: 165% verdana,arial,helvetica
				}
				h2 {
					margin-top: 1em; margin-bottom: 0.5em; font: bold 125% verdana,arial,helvetica
				}
				h3 {
					margin-bottom: 0.5em; font: bold 115% verdana,arial,helvetica
				}
				.Failure {
					font-weight:bold; color:red;
				}
				
	
				img
				{
				  border-width: 0px;
				}
				
				.expand_link
				{
				   position=absolute;
				   right: 0px;
				   width: 27px;
				   top: 1px;
				   height: 27px;
				}
				
				.page_details
				{
				   display: none;
				}
                                
                                .page_details_expanded
                                {
                                    display: block;
                                    display/* hide this definition from  IE5/6 */: table-row;
                                }

                td textarea {
                    background-color: #eeeee0; 
                    border: none;
                    width: 100%;
                    height: 100%;
                }
                td iframe {
                    border: none;
                    width: 100%;
                    height: 100%;
                    resize: both;
                    overflow: auto;
                }
			</style>
			<script language="JavaScript"><![CDATA[
                           function expand(details_id)
			   {
			      
			      document.getElementById(details_id).className = "page_details_expanded";
			   }
			   
			   function collapse(details_id)
			   {
			      
			      document.getElementById(details_id).className = "page_details";
			   }
			   
			   function change(details_id)
			   {
			      if(document.getElementById(details_id+"_image").src.match("expand"))
			      {
			         document.getElementById(details_id+"_image").src = "collapse.png";
			         expand(details_id);
			      }
			      else
			      {
			         document.getElementById(details_id+"_image").src = "expand.png";
			         collapse(details_id);
			      } 
                           }
			]]></script>
		</head>
		<body>
		
			<xsl:call-template name="pageHeader" />

			<xsl:call-template name="pagelist" />
			<hr size="1" width="95%" align="center" />
			
			<xsl:call-template name="detail" />

		</body>
	</html>
</xsl:template>

<xsl:template name="pageHeader">
	<h1><xsl:value-of select="$titleReport" /></h1>
	<table width="100%">
		<tr>
			<td align="left">Date report: <xsl:value-of select="$dateReport" /></td>
			<td align="right">Designed for use with <a href="http://jmeter.apache.org/">JMeter</a> and <a href="http://ant.apache.org">Ant</a>.</td>
		</tr>
	</table>
	<hr size="1" />
</xsl:template>

<xsl:template name="pagelist">
	<h2>Transactions</h2>
	<table align="center" class="details" border="0" cellpadding="5" cellspacing="2" width="95%">
		<tr valign="top">
			<th>URL</th>
			<th># Failures</th>
			<th>Average Response Time</th>
			<th>Min Response Time</th>
			<th>Max Response Time</th>
			<th></th>
		</tr>
        <xsl:for-each select="/testResults/*[count(. | key('results-by-label', @lb)[1]) = 1]">
			<xsl:variable name="label" select="@lb" />
			<xsl:variable name="count" select="count(../*[@lb = current()/@lb])" />
			<xsl:variable name="failureCount" select="count(../*[@lb = current()/@lb][attribute::s='false'])" />


			<xsl:variable name="totalTime" select="sum(../*[@lb = current()/@lb]/@t)" />
			<xsl:variable name="averageTime" select="$totalTime div $count" />
			<xsl:variable name="minTime">
				<xsl:call-template name="min">
					<xsl:with-param name="nodes" select="../*[@lb = current()/@lb]/@t" />
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="maxTime">
				<xsl:call-template name="max">
					<xsl:with-param name="nodes" select="../*[@lb = current()/@lb]/@t" />
				</xsl:call-template>
			</xsl:variable>
			<tr valign="top">
				<xsl:attribute name="class">
					<xsl:choose>
						<xsl:when test="$failureCount &gt; 0">Failure</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<td>
				<xsl:if test="$failureCount > 0">
				  <a><xsl:attribute name="href">#<xsl:value-of select="$label" /></xsl:attribute>
				  <xsl:value-of select="$label" />
				  </a>
				</xsl:if>
				<xsl:if test="0 >= $failureCount">
				  <xsl:value-of select="$label" />
				</xsl:if>
				</td>
				<td align="center">



					<xsl:value-of select="$failureCount" />
				</td>
				<td align="right">





					<xsl:call-template name="display-time">
						<xsl:with-param name="value" select="$averageTime" />
					</xsl:call-template>
				</td>
				<td align="right">
					<xsl:call-template name="display-time">
						<xsl:with-param name="value" select="$minTime" />
					</xsl:call-template>
				</td>
				<td align="right">
					<xsl:call-template name="display-time">
						<xsl:with-param name="value" select="$maxTime" />
					</xsl:call-template>
				</td>
				<td align="center">
				   <a href="">
				      <xsl:attribute name="href"><xsl:text/>javascript:change('page_details_<xsl:value-of select="position()" />')</xsl:attribute>
				      <img src="expand.png" alt="expand/collapse"><xsl:attribute name="id"><xsl:text/>page_details_<xsl:value-of select="position()" />_image</xsl:attribute></img>				      
				   </a>
				</td>
			</tr>
			
                        <tr class="page_details">
                           <xsl:attribute name="id"><xsl:text/>page_details_<xsl:value-of select="position()" /></xsl:attribute>
                           <td colspan="6" bgcolor="#FF0000">
                              <div align="center">
			         <b>Details for Page "<xsl:value-of select="$label" />"</b>
			         <table bordercolor="#000000" bgcolor="#2674A6" border="0"  cellpadding="1" cellspacing="1" width="95%">
			         <tr>
			            <th>Thread</th>
			            <th>Iteration</th>
                        <th>Sample Time</th>
			            <th>Response Time (milliseconds)</th>
			            <th>Response Code</th>
			            <th>Assertions</th>
			            <th>Error Details</th>
			         </tr>
                    
			         <xsl:for-each select="../*[@lb = $label and @tn != $label]">			         			            
			            <tr>
			               <td><xsl:value-of select="@tn" /></td>
			               <td align="center"><xsl:value-of select="position()" /></td>
                           <td align="right"><script>document.write(new Date(<xsl:value-of select="@ts" />).toLocaleString());</script></td>
			               <td align="right"><xsl:value-of select="@t" /></td>
			               <td align="center"><xsl:value-of select="@rc | @rs" /> - <xsl:value-of select="@rm" /></td>
                           <td><xsl:for-each select=".//failureMessage"><xsl:value-of select="." /><br/></xsl:for-each></td>
			               <td><xsl:value-of select="@ERROR" /></td>
			            </tr>
			         </xsl:for-each>
			         
			         </table>
			      </div>
                           </td>
                        </tr>
			
		</xsl:for-each>
	</table>
</xsl:template>

<xsl:template name="detail">
	<xsl:variable name="allFailureCount" select="count(/testResults/*[attribute::s='false'])" />

	<xsl:if test="$allFailureCount > 0">
		<h2>Failure Details</h2>

        <xsl:for-each select="/testResults/*[count(. | key('results-by-label', @lb)[1]) = 1]">

			<xsl:variable name="failureCount" select="count(../*[@lb = current()/@lb][attribute::s='false'])" />

			<xsl:if test="$failureCount > 0">
				<h3><xsl:value-of select="@lb" /><a><xsl:attribute name="name"><xsl:value-of select="@lb" /></xsl:attribute></a></h3>

				<table align="center" class="details" border="0" cellpadding="5" cellspacing="2" width="95%">
				<tr valign="top">
					<th style="width:10%;">Thread</th>
					<th>Failure Message</th>
					<xsl:if test="$showData = 'y'">
					   <th style="width:35%;">Request Data</th>
					   <th style="width:35%;">Response Data</th>
					</xsl:if>
				</tr>
			
				<xsl:for-each select="/testResults/*[@lb = current()/@lb][attribute::s='false']">
					<tr>
                        <td><xsl:value-of select="@tn" /></td>
						<td class="message"><xsl:for-each select=".//failureMessage"><xsl:value-of select="." /><br/></xsl:for-each></td>
						<xsl:if test="$showData = 'y'">
                            <td><xsl:value-of select="java.net.URL" /><textarea><xsl:value-of select="queryString" /></textarea></td>
                            <td><xsl:choose>
                                <xsl:when test="contains(responseHeader, 'Content-Type: text/html;')">
                                <iframe srcdoc="" type="text/html">
                                    <xsl:attribute name="srcdoc"><xsl:text/><xsl:value-of select="responseData" /></xsl:attribute>
                                </iframe>
                                </xsl:when>
                                <xsl:otherwise>
                                    <textarea><xsl:value-of select="responseData" /></textarea>
                                </xsl:otherwise>
                            </xsl:choose>
                            </td>
						</xsl:if>
					</tr>
				</xsl:for-each>
				
				</table>
			</xsl:if>

		</xsl:for-each>
	</xsl:if>
</xsl:template>

<xsl:template name="min">
	<xsl:param name="nodes" select="/.." />
	<xsl:choose>
		<xsl:when test="not($nodes)">NaN</xsl:when>
		<xsl:otherwise>
			<xsl:for-each select="$nodes">
				<xsl:sort data-type="number" />
				<xsl:if test="position() = 1">
					<xsl:value-of select="number(.)" />
				</xsl:if>
			</xsl:for-each>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="max">
	<xsl:param name="nodes" select="/.." />
	<xsl:choose>
		<xsl:when test="not($nodes)">NaN</xsl:when>
		<xsl:otherwise>
			<xsl:for-each select="$nodes">
				<xsl:sort data-type="number" order="descending" />
				<xsl:if test="position() = 1">
					<xsl:value-of select="number(.)" />
				</xsl:if>
			</xsl:for-each>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="display-percent">
	<xsl:param name="value" />
	<xsl:value-of select="format-number($value,'0.00%')" />
</xsl:template>

<xsl:template name="display-time">
	<xsl:param name="value" />
	<xsl:value-of select="format-number($value,'0 ms')" />
</xsl:template>
	
</xsl:stylesheet>

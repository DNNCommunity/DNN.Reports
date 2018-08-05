<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">
<html>
<body> 
  <table border="1">
    <tr bgcolor="#9acd32">
      <th>Title</th>
      <th>Description</th>
	  <th>Keyword</th>
    </tr>
    <xsl:for-each select="DocumentElement/QueryResults">
    <tr>
        <xsl:choose>
      <xsl:when test="string-length(Title) > 60">
         <td bgcolor="#ff0000">
         <xsl:value-of select="Title"/>
         </td>
      </xsl:when>
      <xsl:otherwise>
         <td><xsl:value-of select="Title"/></td>
      </xsl:otherwise>
      </xsl:choose>
  <xsl:choose>
      <xsl:when test="string-length(Description) > 320">
         <td bgcolor="#ff0000">
         <xsl:value-of select="Description"/>
         </td>
      </xsl:when>
      <xsl:otherwise>
         <td><xsl:value-of select="Description"/></td>
      </xsl:otherwise>
      </xsl:choose>
	   <td><xsl:value-of select="Keyword"/></td>
    </tr>
    </xsl:for-each>
  </table>
</body>
</html>
</xsl:template>
</xsl:stylesheet>


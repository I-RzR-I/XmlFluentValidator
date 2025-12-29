// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-26 19:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-26 19:04
// ***********************************************************************
//  <copyright file="XmlSchemaExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.IO;
using System.Xml;
using System.Xml.Schema;

#endregion

namespace XmlFluentValidator.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An XML schema extensions.
    /// </summary>
    /// =================================================================================================
    internal static class XmlSchemaExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An XmlSchema extension method that converts a XmlSchema to a XmlSchemaSet.
        /// </summary>
        /// <param name="schemaSource">The schemaSource to act on.</param>
        /// <returns>
        ///     SchemaSource(XmlSchema) as an XmlSchemaSet.
        /// </returns>
        /// =================================================================================================
        public static XmlSchemaSet ToSchemaSet(this XmlSchema schemaSource)
        {
            var schemaSet = new XmlSchemaSet();

            schemaSet.Add(schemaSource);

            // Compile the schema set to ensure it's valid
            schemaSet.Compile();

            return schemaSet;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An XmlSchema extension method that XML schema to string.
        /// </summary>
        /// <param name="schema">The schema to act on.</param>
        /// <returns>
        ///     A string.
        /// </returns>
        /// =================================================================================================
        public static string XmlSchemaToString(this XmlSchema schema)
        {
            using var stringWriter = new StringWriter();
            using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
            {
                schema.Write(xmlWriter);
            }

            return stringWriter.ToString();
        }
    }
}
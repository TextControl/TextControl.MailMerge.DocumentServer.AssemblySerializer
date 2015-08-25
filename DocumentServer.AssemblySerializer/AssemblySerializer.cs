//-------------------------------------------------------------------------------------------------------------
// module:          TXTextControl.DocumentServer
//
// description:     This class reads an assembly and creates an XML document that contains the schema
//                  information which can be loaded as an XML data source in TX Words.

//                  It uses the schema for the XSD annotations added by the DataSet class,
//                  "urn:schemas-microsoft-com:xml-msdata".
// copyright:       © Text Control GmbH
// version:         TextControl 22.0
//-------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace TXTextControl.DocumentServer
{
    // -------------------------------------------------------------------------------------------------------
    // Class AssemblySerializer
    // -------------------------------------------------------------------------------------------------------
    public class AssemblySerializer
    {
        // ---------------------------------------------------------------------------------------------------
        // Serialize
        //
        // Param assemblyPath: The full path + name of the assembly to be serialized
        // ---------------------------------------------------------------------------------------------------
        public static string Serialize(string assemblyPath)
        {
            Assembly assembly;

            // load the assembly
            try {
                //assembly = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
                assembly = Assembly.LoadFrom(assemblyPath);
            }
            catch (Exception exc) {
                throw exc;
            }

            // extract the assembly name which is used as the XML parent node
            string sAssemblyName = assembly.FullName.Split(',')[0];

            // create a new XML Document
            XmlDocument xmlDoc = new XmlDocument();

            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("msdata", "urn:schemas-microsoft-com:xml-msdata");
            ns.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

            XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", String.Empty);
            xmlDoc.PrependChild(xmlDec);

            // create parent node
            System.Xml.XmlElement elemRoot = xmlDoc.CreateElement(sAssemblyName);
            xmlDoc.AppendChild(elemRoot);

            // create longer static XML node using a string
            string sSchemaNode = "<xs:schema id=\"" + sAssemblyName + "\" xmlns=\"\" ";
                sSchemaNode += "xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" ";
                sSchemaNode += "xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">";
                sSchemaNode += "<xs:element name=\"" + sAssemblyName + "\" msdata:IsDataSet=\"true\" msdata:Locale=\"en-US\">";
                sSchemaNode += "<xs:complexType><xs:choice minOccurs=\"0\" maxOccurs=\"unbounded\">";
                sSchemaNode += "</xs:choice></xs:complexType></xs:element></xs:schema>";

            XmlDocumentFragment xmlDocFragment = xmlDoc.CreateDocumentFragment();
            xmlDocFragment.InnerXml = sSchemaNode;
            elemRoot.AppendChild(xmlDocFragment);

            // select the "choice" node to include member types
            XmlNode choiceNode = xmlDoc.SelectSingleNode("/descendant::xs:choice[1]", ns);

            // list of all assembly types and relations
            Type[] assemblyTypes = assembly.GetTypes();

            // remove duplicates
            assemblyTypes = assemblyTypes.GroupBy(x => x.Name).Select(y => y.First()).ToArray();

            List<System.Xml.XmlElement> listAnnotations = new List<System.Xml.XmlElement>();
            
            // loop through all classes in assembly
            foreach (Type type in assemblyTypes)
            {
                List<string> listMemberNames = new List<string>();

                if (ContainsSpecialCharacters(type.Name))
                    continue;

                // create XML entries for the classes
                System.Xml.XmlElement typeElement = 
                    xmlDoc.CreateElement("xs", "element", "http://www.w3.org/2001/XMLSchema");
                typeElement.SetAttribute("name", type.Name);
                choiceNode.AppendChild(typeElement);

                System.Xml.XmlElement complexTypeElement = 
                    xmlDoc.CreateElement("xs", "complexType", "http://www.w3.org/2001/XMLSchema");
                typeElement.AppendChild(complexTypeElement);

                System.Xml.XmlElement sequenceElement = 
                    xmlDoc.CreateElement("xs", "sequence", "http://www.w3.org/2001/XMLSchema");
                complexTypeElement.AppendChild(sequenceElement);

                // loop through all properties and create XML nodes for return types
                foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (ContainsSpecialCharacters(property.Name) |
                        listMemberNames.Contains(property.Name) |
                        property.GetMethod == null)
                        continue;

                    if (assemblyTypes.Contains(property.GetMethod.ReturnType))
                    {
                        System.Xml.XmlElement newAnnotationElement = 
                            CreateAnnotationElement(xmlDoc, type.Name, property.GetMethod.ReturnType.Name);
                        if (newAnnotationElement != null)
                            listAnnotations.Add(newAnnotationElement);
                    }

                    // add the member information as XML element
                    sequenceElement.AppendChild(CreateMemberElement(xmlDoc, property.Name, property.GetMethod.ReturnType));
                    listMemberNames.Add(property.Name);
                }

                // loop through all methods and create XML nodes for return types
                foreach (MethodInfo method in type.GetMethods(
                    BindingFlags.Public | BindingFlags.Instance)
                    .Where(y => y.IsSpecialName == false))
                {
                    if (ContainsSpecialCharacters(method.Name) | listMemberNames.Contains(method.Name))
                        continue;

                    if (assemblyTypes.Contains(method.ReturnType))
                        {
                            System.Xml.XmlElement newAnnotationElement = 
                                CreateAnnotationElement(xmlDoc, type.Name, method.ReturnType.Name);
                            if(newAnnotationElement != null)
                                listAnnotations.Add(newAnnotationElement);
                        }

                    // add the member information as XML element
                    sequenceElement.AppendChild(CreateMemberElement(xmlDoc, method.Name, method.ReturnType));
                    listMemberNames.Add(method.Name);
                }

                // write all relations as annotation tags
                foreach (System.Xml.XmlElement element in listAnnotations)
                {
                    xmlDoc.SelectSingleNode(sAssemblyName).FirstChild.AppendChild(element);
                }

                // add an internal id to all classes for relations
                sequenceElement.AppendChild(CreateMemberElement(xmlDoc, "TXID_" + type.Name, typeof(Int32)));
            }

            return xmlDoc.InnerXml;
        }

        // ---------------------------------------------------------------------------------------------------
        // CreateMemberElement
        //
        // Param xmlDoc: The root XML document
        // Param name: The member name of the property or method
        // Param returnType: The Type of the member
        // ---------------------------------------------------------------------------------------------------
        private static System.Xml.XmlElement CreateMemberElement(XmlDocument xmlDoc, string name, Type returnType)
        {
            System.Xml.XmlElement memberElement = xmlDoc.CreateElement("xs", "element", "http://www.w3.org/2001/XMLSchema");
            SetElementAttribute(memberElement, name, returnType);

            return memberElement;
        }

        // ---------------------------------------------------------------------------------------------------
        // CreateAnnotationElement
        //
        // An annotation element is the relation of two classes
        //
        // Param xmlDoc: The root XML document
        // Param parent: The parent "table"
        // Param returnType: The child "table"
        // ---------------------------------------------------------------------------------------------------
        private static System.Xml.XmlElement CreateAnnotationElement(XmlDocument xmlDoc, string parent, string child)
        {
            if (parent == child)
                return null;

            System.Xml.XmlElement AnnotationElement = xmlDoc.CreateElement("xs", "annotation", "http://www.w3.org/2001/XMLSchema");
            System.Xml.XmlElement appinfoElement = xmlDoc.CreateElement("xs", "appinfo", "http://www.w3.org/2001/XMLSchema");

            System.Xml.XmlElement RelationshipElement = xmlDoc.CreateElement("msdata", "Relationship", "urn:schemas-microsoft-com:xml-msdata");
            RelationshipElement.SetAttribute("name", parent + "_" + child);
            RelationshipElement.SetAttribute("parent", "urn:schemas-microsoft-com:xml-msdata", parent);
            RelationshipElement.SetAttribute("child", "urn:schemas-microsoft-com:xml-msdata", child);
            RelationshipElement.SetAttribute("parentkey", "urn:schemas-microsoft-com:xml-msdata", "TXID_" + parent);
            RelationshipElement.SetAttribute("childkey", "urn:schemas-microsoft-com:xml-msdata", "TXID_" + child);

            appinfoElement.AppendChild(RelationshipElement);
            AnnotationElement.AppendChild(appinfoElement);

            return AnnotationElement;
        }

        // ---------------------------------------------------------------------------------------------------
        // SetElementAttribute
        //
        // Sets the attributes of the XMLElement with name and type
        //
        // Param xmlDoc: The root XML document
        // Param propertyName: The name of the member
        // Param type: The member type
        // ---------------------------------------------------------------------------------------------------
        private static void SetElementAttribute(System.Xml.XmlElement xmlElement, string propertyName, Type type)
        {
            xmlElement.SetAttribute("name", propertyName);

            string sType = "xs:string";

            switch (type.Name)
            {
                case "Int32":
                case "Int64":
                    sType = "xs:integer";
                    break;
                case "Single":
                    sType = "xs:float";
                    break;
                case "Boolean":
                    sType = "xs:boolean";
                    break;
                case "Byte":
                case "SByte":
                    sType = "xs:byte";
                    break;
                case "Decimal":
                    sType = "xs:decimal";
                    break;
                case "Double":
                    sType = "xs:double";
                    break;
                case "UInt32":
                    sType = "xs:unsignedInt";
                    break;
                case "UInt64":
                    sType = "xs:unsignedLong";
                    break;
                case "Int16":
                    sType = "xs:short";
                    break;
                case "UInt16":
                    sType = "xs:unsignedShort";
                    break;
            }

            xmlElement.SetAttribute("type", sType);
        }

        // ---------------------------------------------------------------------------------------------------
        // ContainsSpecialCharacters
        //
        // Checks the correct format of a string. Unicode and special chars are not allowed
        //
        // Param input: The member name
        // ---------------------------------------------------------------------------------------------------
        public static bool ContainsSpecialCharacters(string input)
        {
            return Regex.IsMatch(input, @"[^0-9a-zA-Z]+");
        }
    }
}

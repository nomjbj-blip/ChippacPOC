using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace DACrux.SP.Common
{
    public class Analysis
    {
        #region " Member Field & Property "

        private static Analysis analysis = null;

        public string TargetFileNamingRule { get; set; }
        public PatternModeItem PatternMode { get; set; }
        public encoding encoding { get; set; }
        private EntityCollection entities = new EntityCollection();
        public Dictionary<string, List<Token>> values = new Dictionary<string, List<Token>>();
        public EntityCollection Entities
        {
            get { return entities; }
        }

        private SectionCollection sections = new SectionCollection();
        public SectionCollection Sections
        {
            get { return sections; }
        }

        private ITaskCollection tasks = new ITaskCollection();
        public ITaskCollection Tasks
        {
            get { return tasks; }
            set { tasks = value; }
        }

        private List<Script> scripts = new List<Script>();
        public List<Script> Scripts
        {
            get { return scripts; }
            set { scripts = value; }
        }


        private Dictionary<string, RegexNavigator> dicFileNavigator = new Dictionary<string, RegexNavigator>();
        public Dictionary<string, RegexNavigator> Navigators
        {
            get { return dicFileNavigator; }
        }

        public RegexNavigator CurrentFileNavigator
        {
            get
            {
                if (string.IsNullOrEmpty(CurrentFile))
                    return null;

                return dicFileNavigator[CurrentFile];
            }
        }

        public string CurrentFile { get; set; }
        public int CharCount { get; set; }
        private Dictionary<string, string> sampleFiles = new Dictionary<string, string>();
        public Dictionary<string, string> SampleFiles
        {
            get { return sampleFiles; }
        }
        //하나마이크론 샘플 변환용 함수
        private Dictionary<string, string> HanasampleFiles = new Dictionary<string, string>();
        public Dictionary<string, string> HanaSampleFiles
        {
            get { return HanasampleFiles; }
        }
        public bool Dirty { get; set; }
        public string FilePath { get; set; }
        public bool Saved { get; set; }
        public bool XMLSaved { get; set; }
        /// <summary>
        /// Flat_Zone
        /// </summary>
        public string strBottom = string.Empty;
        public string strTop = string.Empty;
        public string strRight = string.Empty;
        public string strLeft = string.Empty;

        public string WaferStp = string.Empty;
        public string ProductStp = string.Empty;
        public string FlatStp = string.Empty;
        public string RunStp = string.Empty;
        public string BinStp = string.Empty;

        public string XLength = string.Empty;
        public string YLength = string.Empty;

        public string WaferStr = string.Empty;
        public string ProductStr = string.Empty;
        public string FlatStr = string.Empty;
        public string RunStr = string.Empty;


        private ConnectionInformation connInfo = null;
        public ConnectionInformation ConnectionInfo
        {
            get { return connInfo; }
            set { connInfo = value; }
        }

        #endregion

        #region " Creator "

        private Analysis()
        {
            analysis = this;
            analysis.Entities.ItemAdded += new ItemAddedEventHandler<RegexEntity>(Entities_ItemAdded);
            analysis.Entities.ItemRemoved += new ItemRemovedEventHandler<RegexEntity>(Entities_ItemRemoved);
            analysis.Sections.ItemAdded += new ItemAddedEventHandler<Section>(Sections_ItemAdded);
            analysis.Sections.ItemRemoved += new ItemRemovedEventHandler<Section>(Sections_ItemRemoved);
        }

        #endregion

        #region " Method "

        #region [ Common ]

        public static Analysis GetInstance()
        {
            if (analysis == null)
                analysis = new Analysis();

            return analysis;
        }

        public static void Close()
        {
            analysis.Sections.Clear();
            analysis.Entities.Clear();
            analysis.Navigators.Clear();
            analysis.SampleFiles.Clear();
            analysis = null;
        }

        #endregion

        #region [ Capture ]

        public List<Token> GetCaptures(ISectionItem item)
        {
            try
            {
                return analysis.CurrentFileNavigator.GetCaptures(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Token> GetCaptures(RegexEntity entity)
        {
            try
            {
                return analysis.CurrentFileNavigator.GetCaptures(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Token> GetCaptures(RegexEntity entity, string valueGroupName)
        {
            try
            {
                return analysis.CurrentFileNavigator.GetCaptures(entity, valueGroupName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Token GetCapture(ISectionItem item, int matchIndex)
        {
            try
            {
                if (item is RegexEntity)
                    return GetCapture((item as RegexEntity), matchIndex);
                else
                    return GetCapture((item as RegexSection), matchIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Token GetCapture(RegexEntity entity, int matchIndex)
        {
            try
            {
                return analysis.CurrentFileNavigator.GetCapture(entity, matchIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Token GetCapture(RegexSection section, int matchIndex)
        {
            try
            {
                return analysis.CurrentFileNavigator.GetCapture(section, matchIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ IO ]

        public void OpenFormatFile(FormatFileType formatType, string formatFile)
        {
            #region " Local Variable "

            XmlDocument xmlDoc = null;
            XmlNodeList entityNodes = null;
            XmlNodeList sectionNodes = null;
            XmlNodeList sampleFileNodes = null;
            XmlNode node = null;
            XmlNode dbSerializedNode = null;
            XmlTextReader xtr = null;

            string strName = string.Empty;
            string strType = string.Empty;
            string strData = string.Empty;

            RegexEntity entity = null;
            RegexSection section = null;

            #endregion

            try
            {
                #region " XML Loading "

                xmlDoc = new XmlDocument();
                xmlDoc.Load(formatFile);

                #endregion

                #region " Analysis Loading "

                node = xmlDoc.SelectSingleNode("ANALYSIS");
                if (node == null)
                    throw new Exception("Invalid SmartParserFile format!");

                if (node.Attributes["PatternMode"] != null)
                    PatternMode = (PatternModeItem)Enum.Parse(typeof(PatternModeItem), node.Attributes["PatternMode"].Value);

                if (node.Attributes["TargetFileNamingRule"] != null)
                    TargetFileNamingRule = node.Attributes["TargetFileNamingRule"].Value;
                else
                    TargetFileNamingRule = string.Empty;

                if (node.Attributes["Encoding"] != null)
                    encoding = (encoding)Enum.Parse(typeof(encoding), node.Attributes["Encoding"].Value);
                
                CharCount = int.Parse(node.Attributes["CharCount"].Value);

                if (node.Attributes["BOTTOM"] != null)
                    strBottom = node.Attributes["BOTTOM"].Value;
                if (node.Attributes["LEFT"] != null)
                    strLeft = node.Attributes["LEFT"].Value;
                if (node.Attributes["TOP"] != null)
                    strTop = node.Attributes["TOP"].Value;
                if (node.Attributes["RIGHT"] != null)
                    strRight = node.Attributes["RIGHT"].Value;

                Saved = true;
                Dirty = false;
                FilePath = formatFile;

                #endregion

                #region " SampleFile Loading "

                if (formatType == FormatFileType.Formatter)
                {
                    sampleFileNodes = node.SelectNodes("SAMPLE_FILE");
                    //string tempFolderPath = Utility.CombinedPath(Application.StartupPath, "TEMP");
                    string strFileName = string.Empty;
                    foreach (XmlNode sampleFileNode in sampleFileNodes)
                    {
                        strFileName = sampleFileNode.Attributes["Name"].Value;

                        //using (StreamWriter sw = new StreamWriter(Utility.CombinedPath(tempFolderPath, strFileName), false))
                        //    sw.Write(sampleFileNode.InnerText);

                        //SampleFiles.Add(strFileName, string.Empty);

                        SampleFiles.Add(strFileName, sampleFileNode.InnerText);
                        HanasampleFiles.Add(strFileName + "_HANA", sampleFileNode.InnerText);
                    }
                }

                #endregion

                #region " Entity Loading "

                entityNodes = node.SelectNodes("REGEX_ENTITY");
                foreach (XmlNode entityNode in entityNodes)
                {
                    entity = new RegexEntity();
                    entity.Name = entityNode.Attributes["Name"].Value;
                    entity.Force = System.Convert.ToBoolean(entityNode.Attributes["Force"].Value);
                    if (entityNode.Attributes["Default"] != null)
                        entity.DefaultValue = entityNode.Attributes["Default"].Value;
                    entity.Label = entityNode.Attributes["Label"].Value;
                    entity.RegexString = entityNode.Attributes["RegexString"].Value;
                    entity.RegexOption = (RegexOptions)Enum.Parse(typeof(RegexOptions), entityNode.Attributes["RegexOption"].Value);
                    entity.RegexValueGroupName = entityNode.Attributes["RegexValueGroupName"].Value;
                    entity.RegexValueSeperator = entityNode.Attributes["RegexValueSeperator"].Value;
                    entity.ValueType = string.IsNullOrEmpty(entityNode.Attributes["ValueType"].Value) ? null : Type.GetType(entityNode.Attributes["ValueType"].Value);

                    if (entityNode.Attributes["StreamMode"] != null)
                        entity.StreamMode = (StreamModeItem)Enum.Parse(typeof(StreamModeItem), entityNode.Attributes["StreamMode"].Value);
                    else
                        entity.StreamMode = StreamModeItem.None;
                    //if (entityNode.Attributes["STP_X"].Value != null)
                    //    entity.STP_X = int.Parse(entityNode.Attributes["STP_X"].Value.ToString());
                    //if (entityNode.Attributes["STP_Y"].Value != null)
                    //    entity.STP_X = int.Parse(entityNode.Attributes["STP_Y"].Value.ToString());

                    if (entityNode.Attributes["RowNum"] != null)
                        entity.RowNum = entityNode.Attributes["RowNum"].Value;
                    Entities.Add(entity);
                }

                #endregion

                #region " Section Loading "

                sectionNodes = node.SelectNodes("REGEX_SECTION");
                foreach (XmlNode sectionNode in sectionNodes)
                {
                    section = new RegexSection();
                    section.Name = sectionNode.Attributes["Name"].Value;
                    section.Force = System.Convert.ToBoolean(sectionNode.Attributes["Force"].Value);
                    section.StartEntity = (RegexEntity)Entities[sectionNode.Attributes["StartEntity"].Value];
                    section.EndEntity = (RegexEntity)Entities[sectionNode.Attributes["EndEntity"].Value];

                    Sections.Add(section);
                }


                strName = string.Empty;
                strData = string.Empty;

                foreach (XmlNode sectionNode in sectionNodes)
                {
                    section = (RegexSection)Sections[sectionNode.Attributes["Name"].Value];

                    strData = sectionNode.Attributes["Items"].Value;
                    if (string.IsNullOrEmpty(strData))
                        continue;

                    foreach (string item in strData.Split(new char[] { '¤' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string[] arrSplit = item.Split('§');

                        strName = arrSplit[1];

                        ISectionItem sectionItem = typeof(RegexEntity).ToString().Equals(arrSplit[0]) ? (ISectionItem)Entities[strName] : (ISectionItem)Sections[strName];
                        if (sectionItem != null)
                        {
                            section.Items.Add(sectionItem);

                        }
                    }
                }

                #endregion

                #region " DB Connection Information "

                try
                {
                    object objTemp = null;
                    Byte[] arrByte = null;
                    int byteLength = 0;
                    dbSerializedNode = node.SelectSingleNode("CONNECTION_INFORMATION");
                    if (dbSerializedNode != null && int.TryParse(dbSerializedNode.Attributes["Length"].Value, out byteLength))
                    {
                        arrByte = new byte[byteLength];
                        xtr = new XmlTextReader(dbSerializedNode.OuterXml, XmlNodeType.Element, null);
                        xtr.MoveToContent();
                        xtr.ReadBinHex(arrByte, 0, arrByte.Length);

                        objTemp = Utility.GetDeserializedData(arrByte);

                        if (objTemp is ConnectionInformation)
                            ConnectionInfo = objTemp as ConnectionInformation;
                        else
                            ConnectionInfo = null;
                    }
                    else
                    {
                        ConnectionInfo = null;
                    }
                }
                catch
                {
                    ConnectionInfo = null;
                }

                #endregion

                #region " Action (Task & Script) Loading "

                try
                {
                    object objTemp = null;
                    Byte[] arrByte = null;
                    int byteLength = 0;
                    dbSerializedNode = node.SelectSingleNode("TASK");
                    if (dbSerializedNode != null && int.TryParse(dbSerializedNode.Attributes["Length"].Value, out byteLength))
                    {
                        arrByte = new byte[byteLength];
                        xtr = new XmlTextReader(dbSerializedNode.OuterXml, XmlNodeType.Element, null);
                        xtr.MoveToContent();
                        xtr.ReadBinHex(arrByte, 0, arrByte.Length);

                        objTemp = Utility.GetDeserializedData(arrByte);

                        if (objTemp is ITaskCollection)
                            Tasks = objTemp as ITaskCollection;
                        else
                            Tasks.Clear();
                    }
                    else
                    {
                        Tasks.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    //Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
                    Tasks.Clear();
                }

                try
                {
                    object objTemp = null;
                    Byte[] arrByte = null;
                    int byteLength = 0;
                    dbSerializedNode = node.SelectSingleNode("SCRIPT");
                    if (dbSerializedNode != null && int.TryParse(dbSerializedNode.Attributes["Length"].Value, out byteLength))
                    {
                        arrByte = new byte[byteLength];
                        xtr = new XmlTextReader(dbSerializedNode.OuterXml, XmlNodeType.Element, null);
                        xtr.MoveToContent();
                        xtr.ReadBinHex(arrByte, 0, arrByte.Length);

                        objTemp = Utility.GetDeserializedData(arrByte);

                        if (objTemp is List<Script>)
                            Scripts = objTemp as List<Script>;
                        else
                            Scripts.Clear();

                        /// 이 상태에서 Script의 TargetTask는 실제 Task와 별개의 인스턴스임.
                        /// 따라서 이름으로 동기화를 해주어야 함.
                        /// 근본적인 해결책으로는... 
                        /// Script의 TargetTask는 내부적으로 Task 이름만 저장하고
                        /// Script의 Args 마찬가지로 내부적으로 Data의 이름만 저장해야함
                        /// 그러려면 ITask 말고 Data 클래스에서도 Name 프로퍼티가 필요하고
                        /// UI도 변경되어야 함.

                        foreach (Script script in Scripts)
                        {
                            for (int i = 0; i < script.Args.Length; i++)
                            {
                                if (script.Args[i] is ITask)
                                    script.Args[i] = Tasks[(script.Args[i] as ITask).Name] as Data;
                            }
                        }
                    }
                    else
                    {
                        Scripts.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    //Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
                    Scripts.Clear();
                }

                #endregion
            }
            catch (Exception ex)
            {
                ex.Data.Add("OpenFormatFile", ex.Message);
                throw ex;
            }
            finally
            {
                if (xmlDoc != null)
                    xmlDoc = null;

                if (xtr != null)
                    xtr.Close();
            }
        }

        public void OpenTargetFile(string targetFile)
        {
            try
            {
                CurrentFile = targetFile;

                Navigators.Clear();
                Navigators.Add(targetFile, new RegexNavigator(Utility.ReadStringFromFile(targetFile, analysis.encoding)));

                foreach (Entity entity in Entities)
                {
                    if (!(entity is RegexEntity))
                        continue;

                    if ((entity as RegexEntity).StreamMode == StreamModeItem.Stream)
                        continue;
                    else
                        foreach (KeyValuePair<string, RegexNavigator> navi in Navigators)
                            navi.Value.SetEntity(entity as RegexEntity);
                }

                foreach (Section section in Sections)
                {
                    if (!(section is RegexSection))
                        continue;

                    foreach (KeyValuePair<string, RegexNavigator> navi in Navigators)
                        navi.Value.SetSection(section as RegexSection);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveFormatFile(FormatFileType formatType, string formatFile, bool Flag)
        {
            XmlTextWriter xtw = null;
            string strContents = string.Empty;

            try
            {
                xtw = new XmlTextWriter(formatFile, Encoding.UTF8);
                xtw.Formatting = Formatting.Indented;
                xtw.Indentation = 5;

                xtw.WriteStartElement("ANALYSIS");
                xtw.WriteAttributeString("PatternMode", PatternMode.ToString());
                xtw.WriteAttributeString("TargetFileNamingRule", TargetFileNamingRule.ToString());

                //if (encoding == null)
                //    encoding = Encoding.UTF8;
                xtw.WriteAttributeString("Encoding", encoding.ToString());
                xtw.WriteAttributeString("CharCount", CharCount.ToString());
                xtw.WriteAttributeString("BOTTOM", strBottom);
                xtw.WriteAttributeString("TOP", strTop);
                xtw.WriteAttributeString("LEFT", strLeft);
                xtw.WriteAttributeString("RIGHT", strRight);
                #region " Entity "
                foreach (RegexEntity entity in Entities)
                {
                    xtw.WriteStartElement("REGEX_ENTITY");
                    xtw.WriteAttributeString("Name", entity.Name);
                    xtw.WriteAttributeString("Parent", entity.Parent == null ? string.Empty : entity.Parent.ToString());

                    if (string.IsNullOrEmpty(entity.RegexString))
                        xtw.WriteAttributeString("Default", entity.DefaultValue == null ? string.Empty : entity.DefaultValue.ToString());

                    else
                        xtw.WriteAttributeString("Default", string.Empty);
                    xtw.WriteAttributeString("Force", entity.Force.ToString());
                    xtw.WriteAttributeString("Label", entity.Label);
                    xtw.WriteAttributeString("RegexString", entity.RegexString);
                    xtw.WriteAttributeString("RegexOption", entity.RegexOption.ToString());
                    xtw.WriteAttributeString("RegexValueGroupName", entity.RegexValueGroupName);
                    xtw.WriteAttributeString("RegexValueSeperator", entity.RegexValueSeperator);
                    xtw.WriteAttributeString("ValueType", entity.ValueType == null ? string.Empty : entity.ValueType.ToString());
                    xtw.WriteAttributeString("StreamMode", entity.StreamMode.ToString());
                    if (entity.Name.Equals("Wafer_ID"))
                    {
                        xtw.WriteAttributeString("RowNum", WaferStp);
                        xtw.WriteAttributeString("strKey", WaferStr);
                    }
                    if (entity.Name.Equals("Product"))
                    {
                        xtw.WriteAttributeString("RowNum", ProductStp);
                        xtw.WriteAttributeString("strKey", ProductStr);
                    }
                    if (entity.Name.Equals("FlatZone"))
                    {
                        xtw.WriteAttributeString("RowNum", FlatStp);
                        xtw.WriteAttributeString("strKey", FlatStr);
                    }
                    if (entity.Name.Equals("Run_ID"))
                    {
                        xtw.WriteAttributeString("RowNum", RunStp);
                        xtw.WriteAttributeString("strKey", RunStr);
                    }
                    if (entity.Name.Equals("bin"))
                        xtw.WriteAttributeString("RowNum", BinStp);

                    xtw.WriteEndElement();
                }
                #endregion

                #region " Section "
                foreach (RegexSection section in Sections)
                {
                    xtw.WriteStartElement("REGEX_SECTION");
                    xtw.WriteAttributeString("Name", section.Name);
                    xtw.WriteAttributeString("StartEntity", section.StartEntity == null ? string.Empty : section.StartEntity.ToString());
                    xtw.WriteAttributeString("EndEntity", section.EndEntity == null ? string.Empty : section.EndEntity.ToString());
                    xtw.WriteAttributeString("Parent", section.Parent == null ? string.Empty : section.Parent.ToString());
                    xtw.WriteAttributeString("Force", section.Force.ToString());
                    xtw.WriteAttributeString("Items", GetSectionItemsStringForSave(section.Items));

                    xtw.WriteEndElement();
                }
                #endregion
                if (Flag)
                {
                    #region " Sample Files "

                    if (formatType == FormatFileType.Formatter)
                    {
                        foreach (var pair in SampleFiles)
                        {
                            try
                            {
                                //using (System.IO.StreamReader sr = new System.IO.StreamReader(Utility.CombinedPath(Application.StartupPath, "TEMP", pair.Key)))
                                //    strContents = sr.ReadToEnd();
                            }
                            catch (Exception ex)
                            {
                                Utility.ShowMessageBox(string.Format("Cannot save a sample file. ({0}) \nError : {1}", pair.Key, ex.Message), MessageBoxIcon.Error);
                                continue;
                            }

                            xtw.WriteStartElement("SAMPLE_FILE");
                            xtw.WriteAttributeString("Name", pair.Key);
                            //xtw.WriteString(strContents);
                            xtw.WriteString(pair.Value);
                            xtw.WriteEndElement();

                        }
                    }

                    #endregion


                    #region " DB Connection Information "

                    xtw.WriteStartElement("CONNECTION_INFORMATION");
                    try
                    {
                        Byte[] arrByte = Utility.GetSerializedData(ConnectionInfo);

                        xtw.WriteAttributeString("Length", arrByte.Length.ToString());
                        xtw.WriteBinHex(arrByte, 0, arrByte.Length);
                    }
                    catch (Exception ex)
                    {
                        Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        xtw.WriteEndElement();
                    }

                    #endregion

                    #region " Action (Task & Script) "

                    xtw.WriteStartElement("TASK");
                    try
                    {
                        Byte[] arrByte = Utility.GetSerializedData(Tasks);

                        xtw.WriteAttributeString("Length", arrByte.Length.ToString());
                        xtw.WriteBinHex(arrByte, 0, arrByte.Length);
                    }
                    catch (Exception ex)
                    {
                        Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        xtw.WriteEndElement();
                    }

                    xtw.WriteStartElement("SCRIPT");
                    try
                    {
                        Byte[] arrByte = Utility.GetSerializedData(Scripts);

                        xtw.WriteAttributeString("Length", arrByte.Length.ToString());
                        xtw.WriteBinHex(arrByte, 0, arrByte.Length);
                    }
                    catch (Exception ex)
                    {
                        Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        xtw.WriteEndElement();
                    }

                    #endregion
                }

                xtw.WriteEndElement();
                xtw.Flush();

                if (Flag)
                    Saved = true;
                else
                    XMLSaved = true;
                FilePath = formatFile;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (xtw != null)
                {
                    xtw.Close();
                    xtw = null;
                }
            }
        }

        private string GetSectionItemsStringForSave(ISectionItemCollection sectionItemList)
        {
            StringBuilder sb = new StringBuilder();

            foreach (ISectionItem item in sectionItemList)
                sb.AppendFormat("¤{0}§{1}", item.GetType().ToString(), item.Name);

            if (sb.Length > 0)
                return sb.ToString(1, sb.Length - 1);
            else
                return string.Empty;
        }

        #endregion

        #region [ Public Getter Method ]

        public IEnumerable<string> GetEntityValues(string entityName)
        {
            return GetEntityValues(entityName, entities[entityName].RegexValueGroupName);
        }

        public IEnumerable<string> GetEntityValues(string entityName, string valueGroupName)
        {
            if (Entities[entityName] == null)
                new ArgumentException(string.Format("No entity named {0} exists .", entityName));

            string strTargetText = analysis.CurrentFileNavigator.TargetText;

            if (string.IsNullOrEmpty(strTargetText))
                return null;

            if (Entities[entityName].StreamMode != StreamModeItem.None)
                return null;

            var entityValues = from Token capture in GetCaptures(Entities[entityName], valueGroupName)
                               select strTargetText.Substring(capture.Index, capture.Length);

            return entityValues;
        }

        public IEnumerable<string> GetEntityValues(string entityName, int parentSectionMatchIndex)
        {
            return GetEntityValues(entityName, entities[entityName].RegexValueGroupName, parentSectionMatchIndex);
        }

        public IEnumerable<string> GetEntityValues(string entityName, string valueGroupName, int parentSectionMatchIndex)
        {
            if (Entities[entityName] == null)
                new ArgumentException(string.Format("No entity named {0} exists .", entityName));

            if (Entities[entityName].StreamMode != StreamModeItem.None)
                return null;

            RegexSection parent = Entities[entityName].Parent as RegexSection;
            if (parent == null)
                return GetEntityValues(entityName, valueGroupName);

            string strTargetText = analysis.CurrentFileNavigator.TargetText;

            if (string.IsNullOrEmpty(strTargetText))
                return null;

            Token parentCapture = GetCapture(parent, parentSectionMatchIndex);
            var entityValues = from Token capture in GetCaptures(Entities[entityName], valueGroupName)
                               where capture.Index >= parentCapture.Index && capture.Index < parentCapture.Index + parentCapture.Length
                               select strTargetText.Substring(capture.Index, capture.Length);

            return entityValues;
        }

        public IEnumerable<string> GetEntityStream(string entityName)
        {
            if (Entities[entityName] == null)
                new ArgumentException(string.Format("No entity named {0} exists .", entityName));

            if (Entities[entityName].StreamMode != StreamModeItem.Stream)
                yield break;

            using (EntityStreamReader esr = new EntityStreamReader(Entities[entityName].RegexString, CurrentFile))
            {
                while (!esr.EndOfStream)
                {
                    yield return esr.ReadEntity();
                }
            }
        }

        public IEnumerable<string> GetEntityStream(string entityName, int parentSectionMatchIndex)
        {
            if (Entities[entityName] == null)
                new ArgumentException(string.Format("No entity named {0} exists .", entityName));

            if (Entities[entityName].StreamMode != StreamModeItem.Stream)
                yield break;

            RegexSection parent = Entities[entityName].Parent as RegexSection;
            Token parentCapture = GetCapture(parent, parentSectionMatchIndex);

            int startIndex = -1;
            int endIndex = -1;

            if (parentCapture != null)
            {
                startIndex = parentCapture.Index;
                endIndex = parentCapture.Index + parentCapture.Length - 1;
            }

            using (EntityStreamReader esr = new EntityStreamReader(Entities[entityName].RegexString, CurrentFile, startIndex, endIndex))
            {
                while (!esr.EndOfStream)
                {
                    yield return esr.ReadEntity();
                }
            }
        }

        #endregion

        #endregion

        #region " Event "

        #endregion

        #region " Event Handler "

        void Entities_ItemAdded(RegexEntity item)
        {
            foreach (KeyValuePair<string, RegexNavigator> navi in analysis.Navigators)
                navi.Value.SetEntity(item as RegexEntity);
        }

        void Entities_ItemRemoved(RegexEntity item)
        {
            foreach (Section section in sections)
            {
                if (section.Items.Contains(item))
                {
                    section.Items.Remove(item);
                    return;
                }
                else
                {
                    if (RemoveISectionItem(section.Items, item))
                        return;
                }
            }

            foreach (KeyValuePair<string, RegexNavigator> navi in analysis.Navigators)
                navi.Value.EntityCaptureCollection.Remove(item as RegexEntity);
        }

        void Sections_ItemRemoved(Section item)
        {
            if (!(item is RegexSection))
                return;

            foreach (KeyValuePair<string, RegexNavigator> navi in analysis.Navigators)
                navi.Value.SectionCaptureCollection.Remove(item as RegexSection);
        }

        void Sections_ItemAdded(Section item)
        {
            if (!(item is RegexSection))
                return;

            foreach (KeyValuePair<string, RegexNavigator> navi in analysis.Navigators)
                navi.Value.SetSection(item as RegexSection);
        }

        private bool RemoveISectionItem(ISectionItemCollection list, ISectionItem item)
        {
            foreach (ISectionItem sectionItem in list)
            {
                if (!(sectionItem is Section))
                    continue;

                if ((sectionItem as Section).Items.Contains(item))
                {
                    (sectionItem as Section).Items.Remove(item);
                    return true;
                }
                else
                    RemoveISectionItem((sectionItem as Section).Items, item);
            }

            return false;
        }

        #endregion
    }
}

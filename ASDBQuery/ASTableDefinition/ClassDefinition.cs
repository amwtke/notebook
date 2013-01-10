using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ASTableDefinition
{

    public class ASTable
    {
        IList _fields;
        string _tableName;
        string _description;
        string _description_cn;
        string _panel;
        public ASTable() { }
        public ASTable(string tableName, IList asFields, string description)
        {
            _fields = asFields;
            _tableName = tableName;
            _description = description;
        }

        public string TableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                _tableName = value;
            }
        }
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        
        public string Description_CN
        {
            get
            {
                return _description_cn;
            }
            set
            {
                _description_cn = value;
            }
        }

        public string Panels
        {
            get 
            {
                return _panel;
            }
            set
            {
                _panel = value;
            }
        }

        public IList Fields
        {
            get
            {
                return _fields;
            }
            set
            {
                _fields = value;
            }
        }
    }

    public class ASField
    {
        string _name;
        string _cnDescription;
        string _dataType;
        string _dataLength;
        string _dataPrecision;
        string _dataScale;
        string _nullable;
        string _columnID;
        string _CHAR_COL_DECL_LENGTH;
        string _CHAR_LENGTH;
        string _CHAR_USED;
        string _tableName;
        public ASField(
                        string tableName,
                        string name,
                        string CNDescription,
                        string DataType,
                        string DataLength,
                        string DataPrecision,
                        string DataScale,
                        string Nullable,
                        string ColumID,
                        string CHAR_COL_DECL_LENGTH,
                        string CHAR_LENGTH,
                        string CHAR_USED
                        )
        {
            _name = name;
            _cnDescription = CNDescription;
            _dataType = DataType;
            _dataLength = DataLength;
            _dataPrecision = DataPrecision;
            _dataScale = DataScale;
            _nullable = Nullable;
            _columnID = ColumID;
            _CHAR_COL_DECL_LENGTH = CHAR_COL_DECL_LENGTH;
            _CHAR_LENGTH = CHAR_LENGTH;
            _CHAR_USED = CHAR_USED;
            _tableName = tableName;
        }
        public ASField() { }

        public string TableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                _tableName = value;
            }
        }

        public string ColumnName
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string CNDescription
        {
            get
            {
                return _cnDescription;
            }
            set
            {
                _cnDescription = value;
            }
        }

        public string DataType
        {
            get
            {
                return _dataType;
            }
            set
            {
                _dataType = value;
            }
        }

        public string DataLength
        {
            get
            {
                return _dataLength;
            }
            set
            {
                _dataLength = value;
            }
        }

        public string DataPrecision
        {
            get
            {
                return _dataPrecision;
            }
            set
            {
                _dataPrecision = value;
            }
        }

        public string DataScale
        {
            get
            {
                return _dataScale;
            }
            set
            {
                _dataScale = value;
            }
        }

        public string Nullable
        {
            get
            {
                return _nullable;
            }
            set
            {
                _nullable = value;
            }
        }

        public string ColumID
        {
            get
            {
                return _columnID;
            }
            set
            {
                _columnID = value;
            }
        }

        public string CHAR_COL_DECL_LENGTH
        {
            get
            {
                return _CHAR_COL_DECL_LENGTH;
            }
            set
            {
                _CHAR_COL_DECL_LENGTH = value;
            }
        }

        public string CHAR_LENGTH
        {
            get
            {
                return _CHAR_LENGTH;
            }
            set
            {
                _CHAR_LENGTH = value;
            }
        }

        public string CHAR_USED
        {
            get
            {
                return _CHAR_USED;
            }
            set
            {
                _CHAR_USED = value;
            }
        }

    }

    public class CH
    {
        public CH() { }
        string _en;
        string _cn;
        string _panel;
        string _catalogue;
        DateTime _updateTime;
        public CH(string en, string cn,string panel,string catalogue,DateTime updateTime)
        {
            _en = en;
            _cn = cn;
            _panel = panel;
            _catalogue = catalogue;
            _updateTime = updateTime;
        }
       
        public string En_Description
        {
            get
            {
                return _en;
            }
            set
            {
                _en = value;
            }
        }

        public string Translation
        {
            get
            {
                return _cn;
            }
            set
            {
                _cn = value;
            }
        }

        public string Panel
        {
            get
            {
                return _panel;
            }
            set
            {
                _panel = value;
            }
        }

        public string CataLogue
        {
            get
            {
                return _catalogue;
            }
            set
            {
                _catalogue = value;
            }
        }

        public DateTime UpdateTime
        {
            get
            {
                return _updateTime;
            }
            set
            {
                _updateTime = value;
            }
        }
    }

    public class Preference
    {
        string _preferenceName;
        string _ValueBPR;
        string _ValueCNNC;
        ArrayList _Description;

        public Preference() { }
        public Preference(string name, string vBPR, string vCNNC, ArrayList des)
        {
            _preferenceName = name;
            _ValueBPR = vBPR;
            _ValueCNNC = vCNNC;
            _Description = des;
        }

        public string PreferenceName
        {
            get
            {
                return _preferenceName;
            }
            set
            {
                _preferenceName = value;
            }
        }

        public string BPR_Value
        {
            get
            {
                return _ValueBPR;
            }
            set
            {
                _ValueBPR = value;
            }
        }

        public string CNNC_Value
        {
            get
            {
                return _ValueCNNC;
            }
            set
            {
                _ValueCNNC = value;
            }
        }

        public ArrayList DescArray
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }
    }

    public class PreferenceAll
    {
        string _preferenceName;
        string _preferenceDisplayName;
        string _preferenceValue;
        string _activeIND;
        string _catalogue;
        List<string> _descList;

        public PreferenceAll() { }
        public PreferenceAll(string name, string displayName, string value, string active, string catalogue, List<string> list)
        {
            _preferenceName = name;
            _preferenceDisplayName = displayName;
            _preferenceValue = value;
            _activeIND = active;
            _catalogue = catalogue;
            _descList = list;
        }

        public string PreferenceName
        {
            get
            {
                return _preferenceName;
            }
            set
            {
                _preferenceName = value;
            }
        }

        public string PreferenceDisplayName
        {
            get
            {
                return _preferenceDisplayName;
            }
            set
            {
                _preferenceDisplayName = value;
            }
        }

        public string PreferenceValue
        {
            get
            {
                return _preferenceValue;
            }
            set
            {
                _preferenceValue = value;
            }
        }

        public string ActiveIND
        {
            get
            {
                return _activeIND;
            }
            set
            {
                _activeIND = value;
            }
        }

        public string Catalogue
        {
            get
            {
                return _catalogue;
            }
            set
            {
                _catalogue = value;
            }
        }

        public List<string> DescriptionList
        {
            get
            {
                return _descList;
            }
            set
            {
                _descList = value;
            }
        }
    }

    public class NoteBook
    {
        string _item;
        string _category;
        string _url;
        DateTime _updateTime;
        List<string> _description;
        string _rtf;
        public NoteBook() { }
        public NoteBook(string item, string category, string url, DateTime time,List<string> list)
        {
            _item = item;
            _category = category;
            _url = url;
            _updateTime = time;
            _description = list;
        }

        public NoteBook(string item, string category, string url, DateTime time, List<string> list,string rtf)
        {
            _item = item;
            _category = category;
            _url = url;
            _updateTime = time;
            _description = list;
            _rtf = rtf;
        }
        public string Item
        {
            get{return _item;}
            set{_item = value;}
        }
        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }
        public string URL
        {
            get { return _url; }
            set { _url = value; }
        }
        public DateTime UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }

        public List<string> Content
        {
            get { return _description; }
            set { _description = value; }
        }

        public string RTF
        {
            get { return _rtf; }
            set { _rtf = value; }
        }
    }

    public class HelpInfor
    {
        string _ELEM_NBR;
        string _ELEM_NAME;
        string _ELEM_LENGTH;
        string _ELEM_EDIT_TYPE;
        string _CODE_QUALIFIER;
        string _CODE_DESC_LNG;
        string _CODE_LOCKED_SW;
        string _VALIDATION_ROUTINE;
        string _ELEM_DESC_NAME;
        string _ELEM_DEFINITION;

        public HelpInfor()
        {
        }
        public HelpInfor(string nbr, string name, string length, string edit_type, string code_qualifier, string desc_lng, string locked_sw, string routine, string desc_name, string definition)
        {
            _ELEM_NBR = nbr;
            _ELEM_NAME = name;
            _ELEM_LENGTH = length;
            _ELEM_EDIT_TYPE = edit_type;
            _CODE_QUALIFIER = code_qualifier;
            _CODE_DESC_LNG = desc_lng;
            _CODE_LOCKED_SW = locked_sw;
            _VALIDATION_ROUTINE = routine;
            _ELEM_DESC_NAME = desc_name;
            _ELEM_DEFINITION = definition;
        }

        public string ELEM_NBR
        {
            get { return _ELEM_NBR; }
            set { _ELEM_NBR = value; }
        }
        public string ELEM_NAME
        {
            get { return _ELEM_NAME; }
            set { _ELEM_NAME = value; }
        }
        public string ELEM_LENGTH
        {
            get { return _ELEM_LENGTH; }
            set { _ELEM_LENGTH = value; }
        }
        public string ELEM_EDIT_TYPE
        {
            get { return _ELEM_EDIT_TYPE; }
            set { _ELEM_EDIT_TYPE = value; }
        }
        public string CODE_QUALIFIER
        {
            get { return _CODE_QUALIFIER; }
            set { _CODE_QUALIFIER = value; }
        }
        public string CODE_DESC_LNG
        {
            get { return _CODE_DESC_LNG; }
            set { _CODE_DESC_LNG = value; }
        }
        public string CODE_LOCKED_SW
        {
            get { return _CODE_LOCKED_SW; }
            set { _CODE_LOCKED_SW = value; }
        }

        public string VALIDATION_ROUTINE
        {
            get { return _VALIDATION_ROUTINE; }
            set { _VALIDATION_ROUTINE = value; }
        }

        public string ELEM_DESC_NAME
        {
            get { return _ELEM_DESC_NAME; }
            set { _ELEM_DESC_NAME = value; }
        }

        public string ELEM_DEFINITION
        {
            get { return _ELEM_DEFINITION; }
            set { _ELEM_DEFINITION = value; }
        }
    }
}


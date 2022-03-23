

namespace Syscaf.Service.Helpers
{

    public class DataTableVM
    {
        #region datatable
        public string draw { get; set; }

        public int start { get; set; }
        public int length { get; set; }
        #endregion

        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public object data { get; set; }

        public int iDisplayStart { get; set; }
        public string Buscar { get; set; }


        //#region "Result"

        //public int iTotalRecords { get; set; }

        //public int iTotalDisplayRecords { get; set; }

        //public object aaData { get; set; }

        //#endregion "Result"

        //#region "Multiples Columnas"

        //public string sSearch_0 { get; set; }
        //public bool bSortable_0 { get; set; }
        //public string mDataProp_0 { get; set; }

        //public string sSearch_1 { get; set; }
        //public bool bSortable_1 { get; set; }
        //public string mDataProp_1 { get; set; }

        //public string sSearch_2 { get; set; }
        //public bool bSortable_2 { get; set; }
        //public string mDataProp_2 { get; set; }

        //public string sSearch_3 { get; set; }
        //public bool bSortable_3 { get; set; }
        //public string mDataProp_3 { get; set; }

        //public string sSearch_4 { get; set; }
        //public bool bSortable_4 { get; set; }
        //public string mDataProp_4 { get; set; }

        //public string sSearch_5 { get; set; }
        //public bool bSortable_5 { get; set; }
        //public string mDataProp_5 { get; set; }

        //public string sSearch_6 { get; set; }
        //public bool bSortable_6 { get; set; }
        //public string mDataProp_6 { get; set; }

        //public string sSearch_7 { get; set; }
        //public bool bSortable_7 { get; set; }
        //public string mDataProp_7 { get; set; }

        //#endregion

    }

}

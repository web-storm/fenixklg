//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace testapp.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblBalance
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsBank { get; set; }
        public string Code { get; set; }
        public int Balance { get; set; }
    
        public virtual tblUsers tblUsers { get; set; }
    }
}

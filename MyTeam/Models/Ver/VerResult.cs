﻿using System;
using System.ComponentModel.DataAnnotations;

namespace MyTeam.Models
{
    public class VerResult
    {
        [Display(Name = "系统编号")]
        public string SysNO { get; set; }
        
        [Display(Name = "系统名称")]
        public string SysName { get; set; }

        [Display(Name = "版本发布频率（月）")]
        public int ReleaseFreq { get; set; }

        [Display(Name = "发布时间（计划）")]
        [DataType(DataType.Date)]
        public DateTime? PublishTime { get; set; }

        [Display(Name = "发布版本号")]
        public string VerNo { get; set; }

    }
}
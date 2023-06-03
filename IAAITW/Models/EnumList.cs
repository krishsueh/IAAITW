using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class EnumList
    {
        public enum GenderType
        {
            男,
            女
        }

        public enum MemberType
        {
            正式會員,
            準會員,
            個人贊助會員,
            學生會員
        }
    }
}
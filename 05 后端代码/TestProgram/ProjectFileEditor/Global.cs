using System;
using System.Collections.Generic;
using System.Reflection;
using Test.Common;
using Test.Library;


namespace Test.ProjectFileEditor
{
   
    static class Global
    {
        //public static string LibraryPath;
        //public static List<Type> Classes;
        public static Dictionary<string, Type> Classes;

        public static Project Prj = null;
        //public static Project PrjTemp = null;

        static public MethodInfo[] MethodList(string ClassName)
        {
            Type typ = Classes[ClassName];
            MethodInfo[] method = null;
            MethodInfo[] totalm = typ.GetMethods();
            List<MethodInfo> lm = new List<MethodInfo>();
            foreach (MethodInfo m in totalm)
            {
                if (m.IsPublic && !m.IsVirtual && m.Name != "GetType")
                {
                    lm.Add(m);
                }
            }
            method = lm.ToArray();

            return method;

        }

        static public string GetFullMethodName(MethodInfo method)
        {
            string sM = method.Name;
            string sPT = null;

            if (method.GetParameters().Length > 0)
            {
                sPT = " [";
                foreach (ParameterInfo p in method.GetParameters())
                {
                    sPT += p.ParameterType.Name + ",";
                }
                sPT = sPT.Remove(sPT.Length - 1);
                sPT += "]";
            }
            else
            {
                sPT = " []";
            }
            return sM + sPT;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppConverter
{
    class TypeConfig
    {
        public string name;
        public string ext_header;
        public string ext_cpp;
        public string header_path;
    }
    class Project
    {
        public TypeConfig[] type_settings;
        public string[] export_namespace;
        public string[] export_type;
        public string export_dir;
        public string[] ref_namespace;
        public string precompile_header;
    }
    interface IConverter
    {
        
        Metadata.Model GetModel();
        //string GetTypeHeader(Metadata.DB_Type type);
        HashSet<string> GetTypeDependences(Metadata.DB_Type type);
        HashSet<string> GetMethodBodyDependences(Metadata.DB_Type type);
        HashSet<string> GetTypeDependencesNoDeclareType(Metadata.DB_Type type);
        TypeConfig GetTypeConfig(Metadata.DB_Type type);
        Project GetProject();

        string ExpressionToString(Metadata.Expression.Exp es);
        ITypeConverter GetTypeConverter(Metadata.DB_Type type);
    }
    interface ITypeConverter
    {
        int priority { get; }
        bool SupportType(IConverter Converter, Metadata.DB_Type type);
        bool GetCppTypeName(out string name);
        bool ConvertTypeHeader(IConverter Converter, Metadata.DB_Type type,out string header);
        bool ConvertTypeCpp(IConverter Converter, Metadata.DB_Type type, out string cpp);
        bool ConvertMethodExp(IConverter Converter, Metadata.DB_Type type, Metadata.Expression.MethodExp me,out string exp_string);
        bool ConvertFieldExp(IConverter Converter, Metadata.DB_Type type, Metadata.Expression.FieldExp fe, out string exp_string);
        //bool ConvertIdentifierExp(IConverter Converter, Metadata.DB_Type type, Metadata.Expression.IndifierExp fe, out string exp_string);
    }
}
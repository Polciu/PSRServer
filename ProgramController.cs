using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PSRServer
{

    internal class ProgramController
    {
        private List<Model> objectList = new List<Model>();
        JObject objectToSend = new JObject();
        List<Tuple<String, List<Method>>> classMethodList = new List<Tuple<String, List<Method>>>();

        public ProgramController()
        {
            Glass glass = new Glass();
            objectList.Add(glass);
            Plastic plastic = new Plastic();
            objectList.Add(plastic);
            Metal metal = new Metal();
            objectList.Add(metal);
        }

        public void GetClassesMethods()
        {
            //List<Tuple<String, MethodInfo[], ParameterInfo[]>> classMethods = new List<Tuple<String, MethodInfo[], ParameterInfo[]>>();
            List<Tuple<String, List<Method>>> classMethods = new List<Tuple<String, List<Method>>>();

            //List<String> classNames = new List<String>();
            //List<MethodInfo[]> methodList = new List<MethodInfo[]>();
            //List<ParameterInfo[]> parameterList = new List<ParameterInfo[]>();



            // Pobranie informacji o metodach i nazwach
            foreach(Model model in objectList)
            {
                List<Method> methods = new List<Method>();
                Console.WriteLine(model.productName);
                //Type myType = (typeof(Model));
                Type myType = (typeof(Model));
                switch(model.productName)
                {
                    case "Glass":
                        myType = (typeof(Glass));
                        break;
                }
                MethodInfo[] methodInfo = myType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                //methodList.Add(methodInfo);
                //method.ClassMethodInfo = methodInfo;

                for(int i = 0; i < methodInfo.Length; i++)
                {
                    Method method = new Method();
                    method.ClassMethodInfo = methodInfo[i];

                    ParameterInfo[] paramInfo = methodInfo[i].GetParameters();
                    method.ParamInfo = paramInfo;

                    methods.Add(method);

                }


                //classNames.Add(model.productName);
                Tuple<String, List<Method>> classData = new Tuple<String, List<Method>>(model.productName, methods);
                classMethods.Add(classData);
            }


            // Pobranie informacji o parametrach 
            /*foreach(MethodInfo[] info in methodList)
            {
                for(int i = 0; i < info.Length; i++)
                {
                    ParameterInfo[] paramInfo = info[i].GetParameters();
                    
                    for(int j=0; j < paramInfo.Length; j++)
                    {
                        Console.WriteLine(paramInfo[j].Name);
                        Console.WriteLine(paramInfo[j].ParameterType);
                        Console.WriteLine(paramInfo[j].MetadataToken);
                        Console.WriteLine(paramInfo[j].Position);
                    }

                    parameterList.Add(paramInfo);
                        
                }
            }*/

            // Utworzenie krotek z nazwą klasy, listą metod i parametrami dla każdej z klas.
            /*for(int i = 0; i < methodList.Count; i++)
            {
                Tuple<String, MethodInfo[], ParameterInfo[]> methods =
                    new Tuple<String, MethodInfo[], ParameterInfo[]>(classNames[i], methodList[i], parameterList[i]);
                classMethods.Add(methods);
            }*/



            Console.WriteLine("---------------------------------------------------");
            for(int i = 0; i < classMethods.Count; i++)
            {
                Console.WriteLine("Class name: " + classMethods[i].Item1);

                for(int j = 0; j < classMethods[i].Item2.Count; j++)
                {
                    Console.WriteLine("Method data: ");
                    Console.WriteLine(classMethods[i].Item2[j].ClassMethodInfo.Name);
                    Console.WriteLine(classMethods[i].Item2[j].ClassMethodInfo.ReturnType);

                    Console.WriteLine("Parameters:");
                    for(int k = 0; k < classMethods[i].Item2[j].ParamInfo.Length; k++)
                    {
                        Console.WriteLine(classMethods[i].Item2[j].ParamInfo[k].Name);
                        Console.WriteLine(classMethods[i].Item2[j].ParamInfo[k].ParameterType);
                        Console.WriteLine(classMethods[i].Item2[j].ParamInfo[k].Member);
                    }
                }
            }


            /*Console.WriteLine("NAZWY: ");
            for (int i = 0; i < classMethods.Count; i++)
            {
                Console.WriteLine(classMethods[i].Item1);
            }

            Console.WriteLine("METODY: ");
            for (int i=0; i < classMethods.Count; i++)
            {
                for(int j = 0; j < classMethods[i].Item2.Length; j++)
                {
                    Console.WriteLine(classMethods[i].Item2[j].Name);
                }
            }

            Console.WriteLine("PARAMETRY: ");
            for (int i = 0; i < classMethods.Count; i++)
            {
                for (int j = 0; j < classMethods[i].Item3.Length; j++)
                {
                    Console.WriteLine(classMethods[i].Item3[j].Name);
                }
            }*/

            //
            classMethodList = classMethods;
            //
            objectToSend = serializeMethods(classMethods);

        }

        //public JObject serializeMethods(List<Tuple<String, MethodInfo[], ParameterInfo[]>> classMethods)
        public JObject serializeMethods(List<Tuple<String, List<Method>>> classMethods)
        {
            JObject obj = new JObject();
            JArray objArray = new JArray();

            for(int i = 0; i < classMethods.Count; i++)
            {
                JObject classObj = new JObject();
                JArray methodParams = new JArray();
                JArray methodList = new JArray();

                classObj["name"] = classMethods[i].Item1;

                for(int j = 0; j < classMethods[i].Item2.Count; j++)
                {
                    //Obiekt metody
                    JObject method = new JObject();
                    JArray parameters = new JArray();
                    method["methodName"] = classMethods[i].Item2[j].ClassMethodInfo.Name;
                    method["returnType"] = classMethods[i].Item2[j].ClassMethodInfo.ReturnType.ToString();
                    method["memberType"] = classMethods[i].Item2[j].ClassMethodInfo.MemberType.ToString();

                    for (int k = 0; k < classMethods[i].Item2[j].ParamInfo.Length; k++)
                    {
                        JObject param = new JObject();
                        param["paramName"] = classMethods[i].Item2[j].ParamInfo[k].Name.ToString();
                        param["paramMember"] = classMethods[i].Item2[j].ParamInfo[k].Member.ToString();
                        param["metadataToken"] = classMethods[i].Item2[j].ParamInfo[k].MetadataToken.ToString();
                        param["parameterType"] = classMethods[i].Item2[j].ParamInfo[k].ParameterType.ToString();
                        param["paramPosition"] = classMethods[i].Item2[j].ParamInfo[k].Position;
                        parameters.Add(param);
                    }

                    method["params"] = parameters;
                    methodList.Add(method);
                }

                classObj["methodList"] = methodList;
                objArray.Add(classObj);
            }

            obj["classes"] = objArray;

            /*for (int i=0; i < classMethods.Count; i++)
            {
                JObject classObj = new JObject();
                JArray methodParams = new JArray();
                JArray methodList = new JArray();


                classObj["name"] = classMethods[i].Item1;

                for (int j = 0; j < classMethods[i].Item2.Length; j++)
                {
                    // Obiekt metody
                    JObject method = new JObject();
                    JArray parameters = new JArray();
                    method["methodName"] = classMethods[i].Item2[j].Name;
                    method["returnType"] = classMethods[i].Item2[j].ReturnType.ToString();
                    method["memberType"] = classMethods[i].Item2[j].MemberType.ToString();

                    //Console.WriteLine(method);
                    //Console.WriteLine("PARAM LENGHT: " + classMethods[i].Item3.Length);

                    for(int k=0; k < classMethods[i].Item3.Length; k++)
                    {
                        JObject param = new JObject();
                        param["paramName"] = classMethods[i].Item3[k].Name.ToString();
                        param["paramMember"] = classMethods[i].Item3[k].Member.ToString();
                        param["metadataToken"] = classMethods[i].Item3[k].MetadataToken.ToString();
                        param["parameterType"] = classMethods[i].Item3[k].ParameterType.ToString();
                        param["position"] = classMethods[i].Item3[k].Position;
                        //Console.WriteLine(param);
                        parameters.Add(param);
                    }

                    //Console.WriteLine(parameters);
                    method["params"] = parameters;
                    methodList.Add(method);
                }

                classObj["methodList"] = methodList;
                objArray.Add(classObj);
            }

            obj["classes"] = objArray;*/

            Console.WriteLine("Method list: \n" + obj);

            return obj;
        }

        public string invokeMethod(JObject obj)
        {
            // Deserializacja jsona
            string className = obj["Class"].ToString();
            string method = obj["Method"].ToString();
            JArray paramArray = new JArray();
            paramArray = (JArray)obj["Params"];
            List<Tuple<string, string>> parameters = new List<Tuple<string, string>>();

            for(int i = 0; i < paramArray.Count; i++)
            {
                string paramName = paramArray[i]["ParamName"].ToString();
                string paramValue = paramArray[i]["ParamValue"].ToString();

                Tuple<string, string> paramTuple = new Tuple<string, string>(paramName, paramValue);
                parameters.Add(paramTuple);
            }

            // Wywołanie
            string result = "";
            for(int i = 0; i < objectList.Count; i++)
            {
                if(className == objectList[i].productName)
                {
                    object resultObj = new object();
                    switch (method)
                    {
                        case "Generate":
                            objectList[i].GetType().GetMethod(method).Invoke(objectList[i], null);
                            result = "Data set generated.";
                            break;
                        case "PositivePercentage":
                            if(objectList[i].data.Count <= 0)
                            {
                                result = "Data set not generated! Cannot invoke.";
                            }
                            else
                            {
                                resultObj = objectList[i].GetType().GetMethod(method).Invoke(objectList[i], new object[] { int.Parse(parameters[0].Item2) });
                                result = resultObj.ToString();
                            }
                            //resultObj = objectList[i].GetType().GetMethod(method).Invoke(objectList[i], new object[] { int.Parse(parameters[0].Item2) });
                            //result = resultObj.ToString();
                            break;
                        case "NegativePercentage":
                            if (objectList[i].data.Count <= 0)
                            {
                                result = "Data set not generated! Cannot invoke.";
                            }
                            else
                            {
                                resultObj = objectList[i].GetType().GetMethod(method).Invoke(objectList[i], new object[] { int.Parse(parameters[0].Item2) });
                                result = resultObj.ToString();
                            }
                            //resultObj = objectList[i].GetType().GetMethod(method).Invoke(objectList[i], new object[] { int.Parse(parameters[0].Item2) });
                            //result = resultObj.ToString();
                            break;
                        case "ShowDataSet":
                            if(objectList[i].data.Count <=0)
                            {
                                result = "Data set not generated. Cannot show. ";
                            }
                            else
                            {
                                resultObj = objectList[i].GetType().GetMethod(method).Invoke(objectList[i], null);
                                result = resultObj.ToString();
                            }
                            break;
                        case "GetValuesBetween":
                            if(objectList[i].data.Count <= 0)
                            {
                                result = "Data set not generated. Cannot show. ";
                            }
                            else
                            {
                                resultObj = objectList[i].GetType().GetMethod(method).Invoke(objectList[i], new object[] { int.Parse(parameters[0].Item2), int.Parse(parameters[1].Item2) });
                                result = resultObj.ToString();
                            }
                            break;
                        default:
                            break;
                    }

                    break;
                }
            }

            return result;

        }


        public JObject ObjectToSend
        {
            get => objectToSend;
            set => objectToSend = value;
        }
           
        
    }
}


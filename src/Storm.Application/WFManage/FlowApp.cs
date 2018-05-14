using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Storm.Code;
using Storm.Domain.Entity.WFManage;
using Storm.Domain.IRepository.WFManage;
using Storm.Repository.WFManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Storm.Application.WFManage
{
    public class FlowApp
    {
        private IFlowRepository service = new FlowRepository();
        private IFlowVersionRepository flowVersionService = new FlowVersionRepository();

        public List<FlowEntity> GetAllList(string keyword = "")
        {
            var expression = ExtLinq.True<FlowEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FullName.Contains(keyword));
                expression = expression.Or(t => t.EnCode.Contains(keyword));
            }
            return service.IQueryable(expression).OrderBy(t => t.SortCode).ToList();
        }
        public List<FlowEntity> GetList(string keyword = "")
        {
            var expression = ExtLinq.True<FlowEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FullName.Contains(keyword));
                expression = expression.Or(t => t.EnCode.Contains(keyword));
            }
            expression = expression.And(t => t.DeleteMark != true);
            return service.IQueryable(expression).OrderBy(t => t.SortCode).ToList();
        }
        public List<FlowEntity> GetEnableList(string keyword = "")
        {
            var expression = ExtLinq.True<FlowEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FullName.Contains(keyword));
                expression = expression.Or(t => t.EnCode.Contains(keyword));
            }
            expression = expression.And(t => t.DeleteMark != true && t.EnabledMark == true);
            return service.IQueryable(expression).OrderBy(t => t.SortCode).ToList();
        }
        public FlowEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public FlowVersionEntity GetDesign(string keyValue)
        {
            return flowVersionService.GetNewFlowVersion(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.Id == keyValue);
        }
        public void EnbaledForm(string keyValue)
        {
            FlowEntity flowEntity = GetForm(keyValue);
            if (flowEntity != null && !string.IsNullOrEmpty(flowEntity.Id))
            {
                flowEntity.Modify(keyValue);
                flowEntity.EnabledMark = true;
                service.Update(flowEntity);
            }
            else
            {
                throw new Exception("获取数据异常！");
            }
        }
        public void DisabledForm(string keyValue)
        {
            FlowEntity flowEntity = GetForm(keyValue);
            if (flowEntity != null && !string.IsNullOrEmpty(flowEntity.Id))
            {
                flowEntity.Modify(keyValue);
                flowEntity.EnabledMark = false;
                service.Update(flowEntity);
            }
            else
            {
                throw new Exception("获取数据异常！");
            }
        }
        public void SubmitForm(FlowEntity flowEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                flowEntity.Modify(keyValue);
                service.Update(flowEntity);
            }
            else
            {
                flowEntity.EnabledMark = true;
                flowEntity.Create();
                service.Insert(flowEntity);
            }
        }
        public void SaveDesign(string keyValue, string codes)
        {
            FlowEntity flowEntity = GetForm(keyValue);
            FlowVersionEntity flowVersionEntity = new FlowVersionEntity();
            Random rand = new Random();
            flowVersionEntity.EnCode = "V" + DateTime.Now.ToString("yyyyMMddHHmmss") + rand.Next(0000, 9999);
            flowVersionEntity.Codes = codes;
            if (flowEntity != null && !string.IsNullOrEmpty(flowEntity.Id))
            {
                flowVersionEntity.FlowId = flowEntity.Id;
                JObject obj = JObject.Parse(codes);
                if (obj != null)
                {
                    foreach (KeyValuePair<string, JToken> item in obj)
                    {
                        string key = item.Key;
                        switch (key)
                        {
                            case "title":
                                break;
                            case "nodes":
                                GenFlowNodes(flowVersionEntity, item);
                                break;
                            case "lines":
                                GenFlowLines(flowVersionEntity, item);
                                break;
                            case "areas":
                                GenFlowAreas(flowVersionEntity, item);
                                break;
                            case "initNum":
                                flowVersionEntity.InitNum = (int)item.Value;
                                break;
                        }
                    }
                }
                service.SaveDesign(flowVersionEntity);
            }
            else
            {
                throw new Exception("获取数据异常！");
            }
        }
        private static void GenFlowNodes(FlowVersionEntity flowVersionEntity, KeyValuePair<string, JToken> item)
        {
            if (flowVersionEntity.Nodes == null)
            {
                flowVersionEntity.Nodes = new List<FlowNodeEntity>();
            }
            JObject objnode = JObject.Parse(item.Value.ToString());
            if (objnode != null)
            {
                foreach (KeyValuePair<string, JToken> itemnode in objnode)
                {
                    FlowNodeEntity flowNode = new FlowNodeEntity();
                    flowNode.Id = Guid.NewGuid().ToString();
                    flowNode.MarkName = itemnode.Key.ToString();
                    flowNode.Marked = false;
                    flowNode.IsStartNode = false;
                    flowNode.IsEndNode = false;
                    JObject objnodeitem = JObject.Parse(itemnode.Value.ToString());
                    if (objnodeitem != null)
                    {
                        foreach (KeyValuePair<string, JToken> itemnodeitem in objnodeitem)
                        {
                            if (itemnodeitem.Key == "name")
                            {
                                flowNode.Name = itemnodeitem.Value.ToString();
                            }
                            if (itemnodeitem.Key == "left")
                            {
                                flowNode.Left = (int)itemnodeitem.Value;
                            }
                            if (itemnodeitem.Key == "top")
                            {
                                flowNode.Top = (int)itemnodeitem.Value;
                            }
                            if (itemnodeitem.Key == "width")
                            {
                                flowNode.Width = (int)itemnodeitem.Value;
                            }
                            if (itemnodeitem.Key == "height")
                            {
                                flowNode.Height = (int)itemnodeitem.Value;
                            }
                            if (itemnodeitem.Key == "type")
                            {
                                flowNode.TypeName = itemnodeitem.Value.ToString();
                            }
                            if (itemnodeitem.Key == "steptype")
                            {
                                flowNode.StepType = (int)itemnodeitem.Value;
                            }
                            if (itemnodeitem.Key == "rejecttype")
                            {
                                flowNode.RejectType = (int)itemnodeitem.Value;
                            }
                            if (itemnodeitem.Key == "reviewertype")
                            {
                                flowNode.ReviewerType = (int)itemnodeitem.Value;
                            }
                            if (itemnodeitem.Key == "reviewerusers")
                            {
                                flowNode.ReviewerUser = itemnodeitem.Value.ToString();
                            }
                            if (itemnodeitem.Key == "reviewerorgs")
                            {
                                flowNode.ReviewerOrg = itemnodeitem.Value.ToString();
                            }
                            if (itemnodeitem.Key == "messagetype")
                            {
                                flowNode.MessageType = (int)itemnodeitem.Value;
                            }
                            if (itemnodeitem.Key == "isstart")
                            {
                                flowNode.IsStartNode = (bool)itemnodeitem.Value;
                            }
                            if (itemnodeitem.Key == "isend")
                            {
                                flowNode.IsEndNode = (bool)itemnodeitem.Value;
                            }
                        }
                    }
                    flowVersionEntity.Nodes.Add(flowNode);
                }
            }
        }
        private static void GenFlowLines(FlowVersionEntity flowVersionEntity, KeyValuePair<string, JToken> item)
        {
            if (flowVersionEntity.Lines == null)
            {
                flowVersionEntity.Lines = new List<FlowLineEntity>();
            }
            JObject objnode = JObject.Parse(item.Value.ToString());
            if (objnode != null)
            {
                foreach (KeyValuePair<string, JToken> itemnode in objnode)
                {
                    FlowLineEntity flowLine = new FlowLineEntity();
                    flowLine.Id = Guid.NewGuid().ToString();
                    flowLine.MarkName = itemnode.Key.ToString();
                    flowLine.Marked = false;
                    JObject objnodeitem = JObject.Parse(itemnode.Value.ToString());
                    if (objnodeitem != null)
                    {
                        foreach (KeyValuePair<string, JToken> itemnodeitem in objnodeitem)
                        {
                            if (itemnodeitem.Key == "name")
                            {
                                flowLine.Name = itemnodeitem.Value.ToString();
                            }
                            if (itemnodeitem.Key == "from")
                            {
                                flowLine.FromNode = itemnodeitem.Value.ToString();
                            }
                            if (itemnodeitem.Key == "to")
                            {
                                flowLine.ToNode = itemnodeitem.Value.ToString();
                            }
                            if (itemnodeitem.Key == "type")
                            {
                                flowLine.TypeName = itemnodeitem.Value.ToString();
                            }
                            if (itemnodeitem.Key == "strategiestype")
                            {
                                flowLine.PlotType = (int)itemnodeitem.Value;
                            }
                            if (itemnodeitem.Key == "plot")
                            {
                                flowLine.Plot = itemnodeitem.Value.ToString();
                            }
                            if (itemnodeitem.Key == "sqlplot")
                            {
                                flowLine.SqlPlot = itemnodeitem.Value.ToString();
                            }
                        }
                    }
                    flowVersionEntity.Lines.Add(flowLine);
                }
            }
        }
        private static void GenFlowAreas(FlowVersionEntity flowVersionEntity, KeyValuePair<string, JToken> item)
        {
            if (flowVersionEntity.Areas == null)
            {
                flowVersionEntity.Areas = new List<FlowAreaEntity>();
            }
            JObject objnode = JObject.Parse(item.Value.ToString());
            if (objnode != null)
            {
                foreach (KeyValuePair<string, JToken> itemnode in objnode)
                {
                    FlowAreaEntity flowArea = new FlowAreaEntity();
                    flowArea.Id = Guid.NewGuid().ToString();
                    flowArea.MarkName = itemnode.Key.ToString();
                    flowArea.Marked = false;
                    JObject objnodeitem = JObject.Parse(itemnode.Value.ToString());
                    if (objnodeitem != null)
                    {
                        foreach (KeyValuePair<string, JToken> itemnodeitem in objnodeitem)
                        {
                            if (itemnodeitem.Key == "name")
                            {
                                flowArea.Name = itemnodeitem.Value.ToString();
                            }
                            if (itemnodeitem.Key == "left")
                            {
                                flowArea.Left = (int)itemnodeitem.Value;
                            }
                            if (itemnodeitem.Key == "top")
                            {
                                flowArea.Top = (int)itemnodeitem.Value;
                            }
                            if (itemnodeitem.Key == "width")
                            {
                                flowArea.Width = (int)itemnodeitem.Value;
                            }
                            if (itemnodeitem.Key == "height")
                            {
                                flowArea.Height = (int)itemnodeitem.Value;
                            }
                        }
                    }
                    flowVersionEntity.Areas.Add(flowArea);
                }
            }
        }
        public string GenGooFlows(string keyValue)
        {
            return service.GenGooFlows(keyValue);
        }

        public void SaveStrategies(string flowId, string markName, int flotType, string plots)
        {
            FlowLineEntity flowLineEntity = GetLine(flowId, markName);
            if (flowLineEntity != null && !string.IsNullOrEmpty(flowLineEntity.Id))
            {
                flowLineEntity.PlotType = flotType;
                if (flotType == (int)StrategiesType.Form)
                {
                    flowLineEntity.Plot = plots;
                }
                else
                    if (flotType == (int)StrategiesType.Sql)
                    {
                        flowLineEntity.SqlPlot = plots;
                    }

                flowVersionService.UpdateLine(flowLineEntity);
            }
            else
            {
                throw new Exception("获取数据异常！");
            }
        }
        public List<FlowLineEntity> GetLines(string flowId)
        {
            return flowVersionService.GetLines(flowId);
        }

        public FlowLineEntity GetLine(string flowId, string markName)
        {
            return flowVersionService.GetLine(flowId, markName);
        }

        public List<FlowNodeEntity> GetNodes(string flowId)
        {
            return flowVersionService.GetNodes(flowId);
        }

        public FlowNodeEntity GetNode(string flowId, string markName)
        {
            return flowVersionService.GetNode(flowId, markName);
        }

        public List<FlowAreaEntity> GetAreas(string flowId)
        {
            return flowVersionService.GetAreas(flowId);
        }

        public FlowAreaEntity GetArea(string flowId, string markName)
        {
            return flowVersionService.GetArea(flowId, markName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Data.DataModel;
using Core.Model.Network.DataModel;
using System.Net.Http;
using Core.Model.Methods.Base.DomainModel;
using Core.Model.Methods.CSharp.DomainModel;
using Newtonsoft.Json;

namespace Core.Model.Network.Service
{
	public class SendRequestService : ISendRequestService
	{

		public bool SendData(Node receive_node, DataInvoke data_invoke)
		{
			return true;
		}

		public AssemblyFile GetAssemblyFile(Node receive_node, Guid assembly_file_id)
		{
			using (var client = new HttpClient())
			{
				var content = new StringContent(JsonConvert.SerializeObject(assembly_file_id));

				var response = client.PostAsync(string.Format("http://{0}:{1}/Default/GetAssemblyFile", receive_node.IpAddress, receive_node.Port), content);

				var responseString = response.Result.Content.ReadAsStringAsync().Result;

				return JsonConvert.DeserializeObject<AssemblyFile>(responseString);
			}
		}

		public DataInvoke GetData(Node receive_node, Guid guid)
		{
			using (var client = new HttpClient())
			{
				var content = new StringContent(JsonConvert.SerializeObject(guid));

				var response = client.PostAsync(string.Format("http://{0}:{1}/Default/GetData", receive_node.IpAddress, receive_node.Port), content);

				var responseString = response.Result.Content.ReadAsStringAsync().Result;

				return JsonConvert.DeserializeObject<DataInvoke>(responseString);
			}
		}

		public bool AddData(Node receive_node, DataInvoke data_invoke)
		{
			using (var client = new HttpClient())
			{
				var content = new StringContent(JsonConvert.SerializeObject(data_invoke));

				var response = client.PostAsync(string.Format("http://{0}:{1}/Default/AddData", receive_node.IpAddress, receive_node.Port), content);

				var responseString = response.Result.Content.ReadAsStringAsync().Result;
			}

			return true;
		}
	}
}

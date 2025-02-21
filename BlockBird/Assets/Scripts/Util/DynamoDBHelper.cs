using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public static class DynamoDBHelper
{
    public static T ConvertDynamoDBItem<T>(JToken token) where T : new()
    {
        if (token == null) return default;

        var json = new JObject();

        foreach (var property in token.Children<JProperty>())
        {
            var valueToken = property.Value;

            if (valueToken["S"] != null)
                json[property.Name] = valueToken["S"]; // ���ڿ�

            else if (valueToken["N"] != null)
                json[property.Name] = Convert.ToInt32(valueToken["N"]); // ����

            else if (valueToken["BOOL"] != null)
                json[property.Name] = valueToken["BOOL"]; // �Ҹ���

            else if (valueToken["L"] != null)
            {
                //L (List) Ÿ�� ��ȯ
                var list = new JArray();
                foreach (var item in valueToken["L"])
                {
                    if (item["S"] != null) list.Add(item["S"]); // ���ڿ� ����Ʈ
                    else if (item["N"] != null) list.Add(Convert.ToInt32(item["N"])); // ���� ����Ʈ
                    else if (item["BOOL"] != null) list.Add(item["BOOL"]); // �Ҹ��� ����Ʈ
                    else if (item["M"] != null) list.Add(ConvertDynamoDBItem<JObject>(item["M"])); // Map -> ��ü ��ȯ
                }
                json[property.Name] = list;
            }

            else if (valueToken["M"] != null)
            {
                //M (Map) Ÿ�� ��ȯ (��������� ó��)
                json[property.Name] = ConvertDynamoDBItem<JObject>(valueToken["M"]);
            }
        }

        return json.ToObject<T>(); // JSON�� C# ��ü�� ��ȯ
    }
}

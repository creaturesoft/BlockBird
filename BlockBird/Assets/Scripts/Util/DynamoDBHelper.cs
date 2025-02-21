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
                json[property.Name] = valueToken["S"]; // 문자열

            else if (valueToken["N"] != null)
                json[property.Name] = Convert.ToInt32(valueToken["N"]); // 숫자

            else if (valueToken["BOOL"] != null)
                json[property.Name] = valueToken["BOOL"]; // 불리언

            else if (valueToken["L"] != null)
            {
                //L (List) 타입 변환
                var list = new JArray();
                foreach (var item in valueToken["L"])
                {
                    if (item["S"] != null) list.Add(item["S"]); // 문자열 리스트
                    else if (item["N"] != null) list.Add(Convert.ToInt32(item["N"])); // 숫자 리스트
                    else if (item["BOOL"] != null) list.Add(item["BOOL"]); // 불리언 리스트
                    else if (item["M"] != null) list.Add(ConvertDynamoDBItem<JObject>(item["M"])); // Map -> 객체 변환
                }
                json[property.Name] = list;
            }

            else if (valueToken["M"] != null)
            {
                //M (Map) 타입 변환 (재귀적으로 처리)
                json[property.Name] = ConvertDynamoDBItem<JObject>(valueToken["M"]);
            }
        }

        return json.ToObject<T>(); // JSON을 C# 객체로 변환
    }
}

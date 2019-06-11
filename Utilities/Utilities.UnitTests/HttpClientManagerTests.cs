using Http.Utils.Standard;
using System;
using Xunit;

namespace Utilities.UnitTests
{
    public class HttpClientManagerTests
    {
        IHttpClientManager httpClientManager;
        [Fact]
        public void HttpGetRequestTest()
        {
            httpClientManager = new HttpClientManager();
            var taskResult = httpClientManager.GetAsync<object>("http://localhost:57325/api/values").Result;
            
            Assert.NotNull(taskResult);
        }
    }
}

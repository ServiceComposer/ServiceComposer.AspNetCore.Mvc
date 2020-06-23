using ApprovalTests;
using ApprovalTests.Reporters;
using PublicApiGenerator;
using System.Runtime.CompilerServices;
using ApprovalTests.Namers;
using Xunit;

namespace ServiceComposer.AspNetCore.Mvc.Tests.API
{
    public class APIApprovals
    {
        [Fact]
        [UseReporter(typeof(DiffReporter))]
        [MethodImpl(MethodImplOptions.NoInlining)]
#if NETCOREAPP3_1
        [UseApprovalSubdirectory("NETCOREAPP3_1")]
#else
        [UseApprovalSubdirectory("NETSTANDARD2_0")]
#endif
        public void Approve_API()
        {
            var publicApi = typeof(IHandleResult).Assembly.GeneratePublicApi();

            Approvals.Verify(publicApi);
        }
    }
}
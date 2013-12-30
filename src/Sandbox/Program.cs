using Finviz;
using System.Threading.Tasks;

namespace Sandbox {
    class Program {
        static void Main(string[] args) {
            Task.Run(() => Run()).Wait();
        }

        static async Task Run() {
            var finviz = new FinvizClient();
            var securities = await finviz.GetSecuritiesAsync();
        }
    }
}

using System.Threading;
using System.Threading.Tasks;

public delegate void AnyMethod0args();
public delegate void AnyMethod1args(object arg);

namespace Molodoy.Extensions
{
    public static class MethodExtension
    {
        public static async void RunAfterMillisecondsAsync(this AnyMethod0args methodToRun, int milliseconds)
        {
            await Task.Run(() =>
            {
                Thread.Sleep(milliseconds);
                methodToRun();
            });
        }

        private static async void RunAfterMillisecondsAsync(this AnyMethod1args methodToRun, object argument1, int milliseconds)
        {
            await Task.Run(() =>
            {
                Thread.Sleep(milliseconds);

                methodToRun(argument1);
            });
        }
    }
}
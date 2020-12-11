/*
 * @Author: fasthro
 * @Date: 2020-05-20 22:45:06
 * @Description: callback
 */
 
namespace UFramework.Core
{
    public delegate void UCallback();
    public delegate void UCallback<T>(T arg1);
    public delegate void UCallback<T, U>(T arg1, U arg2);
    public delegate void UCallback<T, U, V>(T arg1, U arg2, V arg3);
    public delegate void UCallback<T, U, V, Z>(T arg1, U arg2, V arg3, Z zrg4);
}
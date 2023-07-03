// Copyright (c) 2018 Inventory Innovations, Inc. - build by Jon P Smith (GitHub JonPSmith)
// Licensed under MIT licence. See License.txt in the project root for license information.

using AutoRegister;

namespace TestAssembly
{
    [Scoped]
    public class MyService : IMyService
    {
        public string IntToString(int num)
        {
            return num.ToString();
        }
    }  
}

// Copyright 2010 Chris Patterson
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Stact
{
    using System;
    using Internal;


    public static class LoopExtensions
    {
        public static void Loop<TState>(this Actor<TState> actor, Action<ReceiveLoop> loopAction)
        {
            var loop = new ReceiveLoopImpl<TState>(actor);

            loopAction(loop);

            loop.Continue();
        }

        public static ReceiveLoop EnableSuspendResume(this ReceiveLoop loop, UntypedActor inbox)
        {
            return loop.Receive<Suspend>(pause =>
                {
                    // we are going to only receive a continue until we get it
                    inbox.Receive<Resume>(x =>
                        {
                            // repeat the loop now
                            loop.Continue();
                        });
                });
        }
    }
}
// Copyright 2007-2008 The Apache Software Foundation.
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
namespace Magnum.Pipeline.Visitors
{
    using System;
    using System.Collections.Generic;
    using Segments;

    public abstract class AbstractPipeVisitor
    {
        protected virtual Pipe Visit(Pipe pipe)
        {
            if (pipe == null)
                return pipe;

            switch (pipe.SegmentType)
            {
                case PipeSegmentType.End:
                    return VisitEnd((EndSegment) pipe);

                case PipeSegmentType.Filter:
                    return VisitFilter((FilterSegment) pipe);

                case PipeSegmentType.Input:
                    return VisitInput((InputSegment) pipe);

                case PipeSegmentType.MessageConsumer:
                    return VisitMessageConsumer((MessageConsumerSegment) pipe);

                case PipeSegmentType.RecipientList:
                    return VisitRecipientList((RecipientListSegment) pipe);

                default:
                    throw new ArgumentException("The pipe node is not a known type: " + pipe.SegmentType,
                        "pipeline");
            }
        }

        protected virtual EndSegment VisitEnd(EndSegment end)
        {
            if (end == null)
                return null;

            return end;
        }

        protected virtual FilterSegment VisitFilter(FilterSegment filter)
        {
            if (filter == null)
                return null;

            Pipe output = Visit(filter.Output);
            if (output != filter.Output)
            {
                return new FilterSegment(output, filter.MessageType);
            }

            return filter;
        }

        protected virtual InputSegment VisitInput(InputSegment input)
        {
            if (input == null)
                return null;

            Pipe pipe = Visit(input.Output);
            if (pipe != input.Output)
            {
                return new InputSegment(pipe);
            }

            return input;
        }

        protected virtual MessageConsumerSegment VisitMessageConsumer(MessageConsumerSegment messageConsumer)
        {
            if (messageConsumer == null)
                return null;

            return messageConsumer;
        }

        protected virtual RecipientListSegment VisitRecipientList(RecipientListSegment recipientList)
        {
            if (recipientList == null)
                return null;

            bool modified = false;
            IList<Pipe> recipients = new List<Pipe>();

            foreach (Pipe recipient in recipientList.Recipients)
            {
                Pipe result = Visit(recipient);
                if (result != recipient)
                {
                    modified = true;
                }

                if (result != null)
                    recipients.Add(result);
            }

            if (modified)
            {
                return new RecipientListSegment(recipientList.MessageType, recipients);
            }

            return recipientList;
        }
    }
}
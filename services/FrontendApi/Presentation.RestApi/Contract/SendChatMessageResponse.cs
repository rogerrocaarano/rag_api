
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contract{
    public class SendChatMessageResponse {

        public SendChatMessageResponse() {
        }

        public Guid conversationId {
            get; set;
        }

        public String message {
            get; set;
        }

        public DateTime createdAt {
            get; set;
        }

    }
}
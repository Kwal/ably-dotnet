﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Linq;

namespace IO.Ably.CustomSerialisers {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MsgPack.Serialization.CodeDomSerializers.CodeDomSerializerBuilder", "0.6.0.0")]
    public class IO_Ably_Types_ProtocolMessageSerializer : MsgPack.Serialization.MessagePackSerializer<IO.Ably.Types.ProtocolMessage> {
        
        private MsgPack.Serialization.MessagePackSerializer<string> _serializer0;
        
        private MsgPack.Serialization.MessagePackSerializer<IO.Ably.Types.ProtocolMessage.MessageAction> _serializer1;
        
        private MsgPack.Serialization.MessagePackSerializer<IO.Ably.ConnectionDetails> _serializer2;
        
        private MsgPack.Serialization.MessagePackSerializer<System.Nullable<long>> _serializer3;
        
        private MsgPack.Serialization.MessagePackSerializer<System.Nullable<int>> _serializer4;
        
        private MsgPack.Serialization.MessagePackSerializer<IO.Ably.ErrorInfo> _serializer5;
        
        private MsgPack.Serialization.MessagePackSerializer<System.Nullable<IO.Ably.Types.ProtocolMessage.MessageFlag>> _serializer6;
        
        private MsgPack.Serialization.MessagePackSerializer<IO.Ably.Message[]> _serializer7;
        
        private MsgPack.Serialization.MessagePackSerializer<long> _serializer8;
        
        private MsgPack.Serialization.MessagePackSerializer<IO.Ably.PresenceMessage[]> _serializer9;
        
        private MsgPack.Serialization.MessagePackSerializer<System.Nullable<System.DateTimeOffset>> _serializer10;
        
        private MsgPack.Serialization.MessagePackSerializer<System.Nullable<IO.Ably.Types.ProtocolMessage.MessageAction>> _serializer11;
        
        public IO_Ably_Types_ProtocolMessageSerializer(MsgPack.Serialization.SerializationContext context) : 
                base(context) {
            MsgPack.Serialization.PolymorphismSchema schema0 = default(MsgPack.Serialization.PolymorphismSchema);
            schema0 = null;
            this._serializer0 = context.GetSerializer<string>(schema0);
            this._serializer1 = context.GetSerializer<IO.Ably.Types.ProtocolMessage.MessageAction>(MsgPack.Serialization.EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethod(context, typeof(IO.Ably.Types.ProtocolMessage.MessageAction), MsgPack.Serialization.EnumMemberSerializationMethod.ByUnderlyingValue));
            MsgPack.Serialization.PolymorphismSchema schema1 = default(MsgPack.Serialization.PolymorphismSchema);
            schema1 = null;
            this._serializer2 = context.GetSerializer<IO.Ably.ConnectionDetails>(schema1);
            MsgPack.Serialization.PolymorphismSchema schema2 = default(MsgPack.Serialization.PolymorphismSchema);
            schema2 = null;
            this._serializer3 = context.GetSerializer<System.Nullable<long>>(schema2);
            MsgPack.Serialization.PolymorphismSchema schema3 = default(MsgPack.Serialization.PolymorphismSchema);
            schema3 = null;
            this._serializer4 = context.GetSerializer<System.Nullable<int>>(schema3);
            MsgPack.Serialization.PolymorphismSchema schema4 = default(MsgPack.Serialization.PolymorphismSchema);
            schema4 = null;
            this._serializer5 = context.GetSerializer<IO.Ably.ErrorInfo>(schema4);
            MsgPack.Serialization.PolymorphismSchema schema5 = default(MsgPack.Serialization.PolymorphismSchema);
            schema5 = null;
            this._serializer6 = context.GetSerializer<System.Nullable<IO.Ably.Types.ProtocolMessage.MessageFlag>>(schema5);
            MsgPack.Serialization.PolymorphismSchema schema6 = default(MsgPack.Serialization.PolymorphismSchema);
            schema6 = null;
            this._serializer7 = context.GetSerializer<IO.Ably.Message[]>(schema6);
            MsgPack.Serialization.PolymorphismSchema schema7 = default(MsgPack.Serialization.PolymorphismSchema);
            schema7 = null;
            this._serializer8 = context.GetSerializer<long>(schema7);
            MsgPack.Serialization.PolymorphismSchema schema8 = default(MsgPack.Serialization.PolymorphismSchema);
            schema8 = null;
            this._serializer9 = context.GetSerializer<IO.Ably.PresenceMessage[]>(schema8);
            MsgPack.Serialization.PolymorphismSchema schema9 = default(MsgPack.Serialization.PolymorphismSchema);
            schema9 = null;
            this._serializer10 = context.GetSerializer<System.Nullable<System.DateTimeOffset>>(schema9);
            MsgPack.Serialization.PolymorphismSchema schema10 = default(MsgPack.Serialization.PolymorphismSchema);
            schema10 = null;
            this._serializer11 = context.GetSerializer<System.Nullable<IO.Ably.Types.ProtocolMessage.MessageAction>>(schema10);
        }
        
        protected override void PackToCore(MsgPack.Packer packer, IO.Ably.Types.ProtocolMessage objectTree)
        {
            var nonNullFields = new bool []
            {
                objectTree.Channel.IsNotEmpty(),
                objectTree.ChannelSerial.IsNotEmpty(),
                objectTree.ConnectionDetails != null,
                objectTree.ConnectionId.IsNotEmpty(),
                objectTree.ConnectionKey.IsNotEmpty(),
                objectTree.ConnectionSerial != null,
                objectTree.Count != null,
                objectTree.Error != null,
                objectTree.Flags != null,
                objectTree.Id.IsNotEmpty(),
                objectTree.Timestamp != null,
                objectTree.Messages != null && objectTree.Messages.Any(x => x.IsEmpty == false),
                objectTree.Presence != null && objectTree.Presence.Any()
            }.Count(x => x) + 2; //One for MsgSerial and one for Action as this is always serialised

            packer.PackMapHeader(nonNullFields);
            this._serializer0.PackTo(packer, "action");
            this._serializer1.PackTo(packer, objectTree.Action);
            if (objectTree.Channel.IsNotEmpty())
            {
                this._serializer0.PackTo(packer, "channel");
                this._serializer0.PackTo(packer, objectTree.Channel);
            }
            if (objectTree.ChannelSerial.IsNotEmpty())
            {
                this._serializer0.PackTo(packer, "channelSerial");
                this._serializer0.PackTo(packer, objectTree.ChannelSerial);
            }
            if (objectTree.ConnectionDetails != null)
            {
                this._serializer0.PackTo(packer, "connectionDetails");
                this._serializer2.PackTo(packer, objectTree.ConnectionDetails);
            }
            if (objectTree.ConnectionId.IsNotEmpty())
            {
                this._serializer0.PackTo(packer, "connectionId");
                this._serializer0.PackTo(packer, objectTree.ConnectionId);
            }
            if (objectTree.ConnectionKey.IsNotEmpty())
            {
                this._serializer0.PackTo(packer, "connectionKey");
                this._serializer0.PackTo(packer, objectTree.ConnectionKey);
            }
            if (objectTree.ConnectionSerial != null)
            {
                this._serializer0.PackTo(packer, "connectionSerial");
                this._serializer3.PackTo(packer, objectTree.ConnectionSerial);
            }
            if (objectTree.Count != null)
            {
                this._serializer0.PackTo(packer, "count");
                this._serializer4.PackTo(packer, objectTree.Count);
            }
            if (objectTree.Error != null)
            {
                this._serializer0.PackTo(packer, "error");
                this._serializer5.PackTo(packer, objectTree.Error);
            }
            if (objectTree.Flags != null)
            {
                this._serializer0.PackTo(packer, "flags");
                this._serializer6.PackTo(packer, objectTree.Flags);
            }
            if (objectTree.Id.IsNotEmpty())
            {
                this._serializer0.PackTo(packer, "id");
                this._serializer0.PackTo(packer, objectTree.Id);
            }
            this._serializer0.PackTo(packer, "msgSerial");
            this._serializer8.PackTo(packer, objectTree.MsgSerial);
            if (objectTree.Messages != null && objectTree.Messages.Any(x => x.IsEmpty == false))
            {
                this._serializer0.PackTo(packer, "messages");
                this._serializer7.PackTo(packer, objectTree.Messages.Where(x => x.IsEmpty == false).ToArray());
            }
            if (objectTree.Presence != null && objectTree.Presence.Any())
            {
                this._serializer0.PackTo(packer, "presence");
                this._serializer9.PackTo(packer, objectTree.Presence);
            }
            if (objectTree.Timestamp != null)
            {
                this._serializer0.PackTo(packer, "timestamp");
                this._serializer10.PackTo(packer, objectTree.Timestamp);
            }
        }
        
        protected override IO.Ably.Types.ProtocolMessage UnpackFromCore(MsgPack.Unpacker unpacker)
        {
            IO.Ably.Types.ProtocolMessage result = default(IO.Ably.Types.ProtocolMessage);
            result = new IO.Ably.Types.ProtocolMessage();
            int itemsCount0 = default(int);
            itemsCount0 = MsgPack.Serialization.UnpackHelpers.GetItemsCount(unpacker);
            for (int i = 0; (i < itemsCount0); i = (i + 1))
            {
                string key = default(string);
                string nullable14 = default(string);
                nullable14 = MsgPack.Serialization.UnpackHelpers.UnpackStringValue(unpacker,
                    typeof(IO.Ably.Types.ProtocolMessage), "MemberName");
                if (((nullable14 == null)
                     == false))
                {
                    key = nullable14;
                }
                else
                {
                    throw MsgPack.Serialization.SerializationExceptions.NewNullIsProhibited("MemberName");
                }
                if ((key == "timestamp"))
                {
                    System.Nullable<System.DateTimeOffset> nullable29 = default(System.Nullable<System.DateTimeOffset>);
                    if ((unpacker.Read() == false))
                    {
                        throw MsgPack.Serialization.SerializationExceptions.NewMissingItem(i);
                    }
                    if (((unpacker.IsArrayHeader == false)
                         && (unpacker.IsMapHeader == false)))
                    {
                        nullable29 = this._serializer10.UnpackFrom(unpacker);
                    }
                    else
                    {
                        MsgPack.Unpacker disposable12 = default(MsgPack.Unpacker);
                        disposable12 = unpacker.ReadSubtree();
                        try
                        {
                            nullable29 = this._serializer10.UnpackFrom(disposable12);
                        }
                        finally
                        {
                            if (((disposable12 == null)
                                 == false))
                            {
                                disposable12.Dispose();
                            }
                        }
                    }
                    if (nullable29.HasValue)
                    {
                        result.Timestamp = nullable29;
                    }
                }
                else
                {
                    if ((key == "presence"))
                    {
                        IO.Ably.PresenceMessage[] nullable28 = default(IO.Ably.PresenceMessage[]);
                        if ((unpacker.Read() == false))
                        {
                            throw MsgPack.Serialization.SerializationExceptions.NewMissingItem(i);
                        }
                        if (((unpacker.IsArrayHeader == false)
                             && (unpacker.IsMapHeader == false)))
                        {
                            nullable28 = this._serializer9.UnpackFrom(unpacker);
                        }
                        else
                        {
                            MsgPack.Unpacker disposable11 = default(MsgPack.Unpacker);
                            disposable11 = unpacker.ReadSubtree();
                            try
                            {
                                nullable28 = this._serializer9.UnpackFrom(disposable11);
                            }
                            finally
                            {
                                if (((disposable11 == null)
                                     == false))
                                {
                                    disposable11.Dispose();
                                }
                            }
                        }
                        if (((nullable28 == null)
                             == false))
                        {
                            result.Presence = nullable28;
                        }
                    }
                    else
                    {
                        if ((key == "msgSerial"))
                        {
                            System.Nullable<long> nullable27 = default(System.Nullable<long>);
                            nullable27 = MsgPack.Serialization.UnpackHelpers.UnpackNullableInt64Value(unpacker,
                                typeof(IO.Ably.Types.ProtocolMessage), "Int64 msgSerial");
                            if (nullable27.HasValue)
                            {
                                result.MsgSerial = nullable27.Value;
                            }
                        }
                        else
                        {
                            if ((key == "messages"))
                            {
                                IO.Ably.Message[] nullable26 = default(IO.Ably.Message[]);
                                if ((unpacker.Read() == false))
                                {
                                    throw MsgPack.Serialization.SerializationExceptions.NewMissingItem(i);
                                }
                                if (((unpacker.IsArrayHeader == false)
                                     && (unpacker.IsMapHeader == false)))
                                {
                                    nullable26 = this._serializer7.UnpackFrom(unpacker);
                                }
                                else
                                {
                                    MsgPack.Unpacker disposable10 = default(MsgPack.Unpacker);
                                    disposable10 = unpacker.ReadSubtree();
                                    try
                                    {
                                        nullable26 = this._serializer7.UnpackFrom(disposable10);
                                    }
                                    finally
                                    {
                                        if (((disposable10 == null)
                                             == false))
                                        {
                                            disposable10.Dispose();
                                        }
                                    }
                                }
                                if (((nullable26 == null)
                                     == false))
                                {
                                    result.Messages = nullable26;
                                }
                            }
                            else
                            {
                                if ((key == "id"))
                                {
                                    string nullable25 = default(string);
                                    nullable25 = MsgPack.Serialization.UnpackHelpers.UnpackStringValue(unpacker,
                                        typeof(IO.Ably.Types.ProtocolMessage), "System.String id");
                                    if (((nullable25 == null)
                                         == false))
                                    {
                                        result.Id = nullable25;
                                    }
                                }
                                else
                                {
                                    if ((key == "flags"))
                                    {
                                        System.Nullable<IO.Ably.Types.ProtocolMessage.MessageFlag> nullable24 =
                                            default(System.Nullable<IO.Ably.Types.ProtocolMessage.MessageFlag>);
                                        if ((unpacker.Read() == false))
                                        {
                                            throw MsgPack.Serialization.SerializationExceptions.NewMissingItem(i);
                                        }
                                        if (((unpacker.IsArrayHeader == false)
                                             && (unpacker.IsMapHeader == false)))
                                        {
                                            nullable24 = this._serializer6.UnpackFrom(unpacker);
                                        }
                                        else
                                        {
                                            MsgPack.Unpacker disposable9 = default(MsgPack.Unpacker);
                                            disposable9 = unpacker.ReadSubtree();
                                            try
                                            {
                                                nullable24 = this._serializer6.UnpackFrom(disposable9);
                                            }
                                            finally
                                            {
                                                if (((disposable9 == null)
                                                     == false))
                                                {
                                                    disposable9.Dispose();
                                                }
                                            }
                                        }
                                        if (nullable24.HasValue)
                                        {
                                            result.Flags = nullable24;
                                        }
                                    }
                                    else
                                    {
                                        if ((key == "error"))
                                        {
                                            IO.Ably.ErrorInfo nullable23 = default(IO.Ably.ErrorInfo);
                                            if ((unpacker.Read() == false))
                                            {
                                                throw MsgPack.Serialization.SerializationExceptions.NewMissingItem(i);
                                            }
                                            if (((unpacker.IsArrayHeader == false)
                                                 && (unpacker.IsMapHeader == false)))
                                            {
                                                nullable23 = this._serializer5.UnpackFrom(unpacker);
                                            }
                                            else
                                            {
                                                MsgPack.Unpacker disposable8 = default(MsgPack.Unpacker);
                                                disposable8 = unpacker.ReadSubtree();
                                                try
                                                {
                                                    nullable23 = this._serializer5.UnpackFrom(disposable8);
                                                }
                                                finally
                                                {
                                                    if (((disposable8 == null)
                                                         == false))
                                                    {
                                                        disposable8.Dispose();
                                                    }
                                                }
                                            }
                                            if (((nullable23 == null)
                                                 == false))
                                            {
                                                result.Error = nullable23;
                                            }
                                        }
                                        else
                                        {
                                            if ((key == "count"))
                                            {
                                                System.Nullable<int> nullable22 = default(System.Nullable<int>);
                                                nullable22 =
                                                    MsgPack.Serialization.UnpackHelpers.UnpackNullableInt32Value(
                                                        unpacker, typeof(IO.Ably.Types.ProtocolMessage),
                                                        "System.Nullable`1[System.Int32] count");
                                                if (nullable22.HasValue)
                                                {
                                                    result.Count = nullable22;
                                                }
                                            }
                                            else
                                            {
                                                if ((key == "connectionSerial"))
                                                {
                                                    System.Nullable<long> nullable21 = default(System.Nullable<long>);
                                                    nullable21 =
                                                        MsgPack.Serialization.UnpackHelpers.UnpackNullableInt64Value(
                                                            unpacker, typeof(IO.Ably.Types.ProtocolMessage),
                                                            "System.Nullable`1[System.Int64] connectionSerial");
                                                    if (nullable21.HasValue)
                                                    {
                                                        result.ConnectionSerial = nullable21;
                                                    }
                                                }
                                                else
                                                {
                                                    if ((key == "connectionKey"))
                                                    {
                                                        string nullable20 = default(string);
                                                        nullable20 =
                                                            MsgPack.Serialization.UnpackHelpers.UnpackStringValue(
                                                                unpacker, typeof(IO.Ably.Types.ProtocolMessage),
                                                                "System.String connectionKey");
                                                        if (((nullable20 == null)
                                                             == false))
                                                        {
                                                            result.ConnectionKey = nullable20;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if ((key == "connectionId"))
                                                        {
                                                            string nullable19 = default(string);
                                                            nullable19 =
                                                                MsgPack.Serialization.UnpackHelpers.UnpackStringValue(
                                                                    unpacker, typeof(IO.Ably.Types.ProtocolMessage),
                                                                    "System.String connectionId");
                                                            if (((nullable19 == null)
                                                                 == false))
                                                            {
                                                                result.ConnectionId = nullable19;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if ((key == "connectionDetails"))
                                                            {
                                                                IO.Ably.ConnectionDetails nullable18 =
                                                                    default(IO.Ably.ConnectionDetails);
                                                                if ((unpacker.Read() == false))
                                                                {
                                                                    throw MsgPack.Serialization.SerializationExceptions
                                                                        .NewMissingItem(i);
                                                                }
                                                                if (((unpacker.IsArrayHeader == false)
                                                                     && (unpacker.IsMapHeader == false)))
                                                                {
                                                                    nullable18 = this._serializer2.UnpackFrom(unpacker);
                                                                }
                                                                else
                                                                {
                                                                    MsgPack.Unpacker disposable7 =
                                                                        default(MsgPack.Unpacker);
                                                                    disposable7 = unpacker.ReadSubtree();
                                                                    try
                                                                    {
                                                                        nullable18 =
                                                                            this._serializer2.UnpackFrom(disposable7);
                                                                    }
                                                                    finally
                                                                    {
                                                                        if (((disposable7 == null)
                                                                             == false))
                                                                        {
                                                                            disposable7.Dispose();
                                                                        }
                                                                    }
                                                                }
                                                                if (((nullable18 == null)
                                                                     == false))
                                                                {
                                                                    result.ConnectionDetails = nullable18;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if ((key == "channelSerial"))
                                                                {
                                                                    string nullable17 = default(string);
                                                                    nullable17 =
                                                                        MsgPack.Serialization.UnpackHelpers
                                                                            .UnpackStringValue(unpacker,
                                                                                typeof(IO.Ably.Types.ProtocolMessage),
                                                                                "System.String channelSerial");
                                                                    if (((nullable17 == null)
                                                                         == false))
                                                                    {
                                                                        result.ChannelSerial = nullable17;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if ((key == "channel"))
                                                                    {
                                                                        string nullable16 = default(string);
                                                                        nullable16 =
                                                                            MsgPack.Serialization.UnpackHelpers
                                                                                .UnpackStringValue(unpacker,
                                                                                    typeof(IO.Ably.Types.ProtocolMessage
                                                                                        ), "System.String channel");
                                                                        if (((nullable16 == null)
                                                                             == false))
                                                                        {
                                                                            result.Channel = nullable16;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if ((key == "action"))
                                                                        {
                                                                            System.Nullable
                                                                                <
                                                                                    IO.Ably.Types.ProtocolMessage.
                                                                                        MessageAction> nullable15 =
                                                                                            default(
                                                                                                System.Nullable
                                                                                                    <
                                                                                                        IO.Ably.Types.
                                                                                                            ProtocolMessage
                                                                                                            .
                                                                                                            MessageAction
                                                                                                        >);
                                                                            if ((unpacker.Read() == false))
                                                                            {
                                                                                throw MsgPack.Serialization
                                                                                    .SerializationExceptions
                                                                                    .NewMissingItem(i);
                                                                            }
                                                                            if (((unpacker.IsArrayHeader == false)
                                                                                 && (unpacker.IsMapHeader == false)))
                                                                            {
                                                                                nullable15 =
                                                                                    this._serializer11.UnpackFrom(
                                                                                        unpacker);
                                                                            }
                                                                            else
                                                                            {
                                                                                MsgPack.Unpacker disposable6 =
                                                                                    default(MsgPack.Unpacker);
                                                                                disposable6 = unpacker.ReadSubtree();
                                                                                try
                                                                                {
                                                                                    nullable15 =
                                                                                        this._serializer11.UnpackFrom(
                                                                                            disposable6);
                                                                                }
                                                                                finally
                                                                                {
                                                                                    if (((disposable6 == null)
                                                                                         == false))
                                                                                    {
                                                                                        disposable6.Dispose();
                                                                                    }
                                                                                }
                                                                            }
                                                                            if (nullable15.HasValue)
                                                                            {
                                                                                result.Action = nullable15.Value;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            unpacker.Skip();
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private static T @__Conditional<T>(bool condition, T whenTrue, T whenFalse)
         {
            if (condition) {
                return whenTrue;
            }
            else {
                return whenFalse;
            }
        }
    }
}

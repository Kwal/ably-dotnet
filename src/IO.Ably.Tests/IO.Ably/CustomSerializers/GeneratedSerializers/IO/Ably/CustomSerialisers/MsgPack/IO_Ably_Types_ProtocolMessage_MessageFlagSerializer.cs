﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IO.Ably.CustomSerialisers.MsgPack {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MsgPack.Serialization.CodeDomSerializers.CodeDomSerializerBuilder", "0.6.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    public class IO_Ably_Types_ProtocolMessage_MessageFlagSerializer : MsgPack.Serialization.EnumMessagePackSerializer<IO.Ably.Types.ProtocolMessage.MessageFlag> {
        
        public IO_Ably_Types_ProtocolMessage_MessageFlagSerializer(MsgPack.Serialization.SerializationContext context) : 
                this(context, MsgPack.Serialization.EnumSerializationMethod.ByName) {
        }
        
        public IO_Ably_Types_ProtocolMessage_MessageFlagSerializer(MsgPack.Serialization.SerializationContext context, MsgPack.Serialization.EnumSerializationMethod enumSerializationMethod) : 
                base(context, enumSerializationMethod) {
        }
        
        protected override void PackUnderlyingValueTo(MsgPack.Packer packer, IO.Ably.Types.ProtocolMessage.MessageFlag enumValue) {
            packer.Pack(((byte)(enumValue)));
        }
        
        protected override IO.Ably.Types.ProtocolMessage.MessageFlag UnpackFromUnderlyingValue(MsgPack.MessagePackObject messagePackObject) {
            return ((IO.Ably.Types.ProtocolMessage.MessageFlag)(messagePackObject.AsByte()));
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
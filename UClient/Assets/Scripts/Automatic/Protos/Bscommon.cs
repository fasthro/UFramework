// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bscommon.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace PBBSCommon {

  /// <summary>Holder for reflection information generated from bscommon.proto</summary>
  public static partial class BscommonReflection {

    #region Descriptor
    /// <summary>File descriptor for bscommon.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BscommonReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg5ic2NvbW1vbi5wcm90bxILUEJfQlNDb21tb24iKAoEVXNlchIOCgZ1c2Vy",
            "SWQYASABKAMSEAoIdXNlck5hbWUYAiABKAkiHAoNRnJhbWVJbnB1dENtZBIL",
            "CgNudW0YASABKAUiSQoKRnJhbWVJbnB1dBIMCgR0aWNrGAEgASgFEi0KCWlu",
            "cHV0Q21kcxgCIAMoCzIaLlBCX0JTQ29tbW9uLkZyYW1lSW5wdXRDbWRiBnBy",
            "b3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::PBBSCommon.User), global::PBBSCommon.User.Parser, new[]{ "UserId", "UserName" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::PBBSCommon.FrameInputCmd), global::PBBSCommon.FrameInputCmd.Parser, new[]{ "Num" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::PBBSCommon.FrameInput), global::PBBSCommon.FrameInput.Parser, new[]{ "Tick", "InputCmds" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///  用户
  /// </summary>
  public sealed partial class User : pb::IMessage<User> {
    private static readonly pb::MessageParser<User> _parser = new pb::MessageParser<User>(() => new User());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<User> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::PBBSCommon.BscommonReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public User() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public User(User other) : this() {
      userId_ = other.userId_;
      userName_ = other.userName_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public User Clone() {
      return new User(this);
    }

    /// <summary>Field number for the "userId" field.</summary>
    public const int UserIdFieldNumber = 1;
    private long userId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long UserId {
      get { return userId_; }
      set {
        userId_ = value;
      }
    }

    /// <summary>Field number for the "userName" field.</summary>
    public const int UserNameFieldNumber = 2;
    private string userName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UserName {
      get { return userName_; }
      set {
        userName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as User);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(User other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (UserName != other.UserName) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0L) hash ^= UserId.GetHashCode();
      if (UserName.Length != 0) hash ^= UserName.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (UserId != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(UserId);
      }
      if (UserName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(UserName);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(UserId);
      }
      if (UserName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(UserName);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(User other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0L) {
        UserId = other.UserId;
      }
      if (other.UserName.Length != 0) {
        UserName = other.UserName;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            UserId = input.ReadInt64();
            break;
          }
          case 18: {
            UserName = input.ReadString();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///  帧->输入命令
  /// </summary>
  public sealed partial class FrameInputCmd : pb::IMessage<FrameInputCmd> {
    private static readonly pb::MessageParser<FrameInputCmd> _parser = new pb::MessageParser<FrameInputCmd>(() => new FrameInputCmd());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<FrameInputCmd> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::PBBSCommon.BscommonReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FrameInputCmd() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FrameInputCmd(FrameInputCmd other) : this() {
      num_ = other.num_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FrameInputCmd Clone() {
      return new FrameInputCmd(this);
    }

    /// <summary>Field number for the "num" field.</summary>
    public const int NumFieldNumber = 1;
    private int num_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Num {
      get { return num_; }
      set {
        num_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as FrameInputCmd);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(FrameInputCmd other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Num != other.Num) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Num != 0) hash ^= Num.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Num != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Num);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Num != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Num);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(FrameInputCmd other) {
      if (other == null) {
        return;
      }
      if (other.Num != 0) {
        Num = other.Num;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            Num = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///  帧-> 输入
  /// </summary>
  public sealed partial class FrameInput : pb::IMessage<FrameInput> {
    private static readonly pb::MessageParser<FrameInput> _parser = new pb::MessageParser<FrameInput>(() => new FrameInput());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<FrameInput> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::PBBSCommon.BscommonReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FrameInput() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FrameInput(FrameInput other) : this() {
      tick_ = other.tick_;
      inputCmds_ = other.inputCmds_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FrameInput Clone() {
      return new FrameInput(this);
    }

    /// <summary>Field number for the "tick" field.</summary>
    public const int TickFieldNumber = 1;
    private int tick_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Tick {
      get { return tick_; }
      set {
        tick_ = value;
      }
    }

    /// <summary>Field number for the "inputCmds" field.</summary>
    public const int InputCmdsFieldNumber = 2;
    private static readonly pb::FieldCodec<global::PBBSCommon.FrameInputCmd> _repeated_inputCmds_codec
        = pb::FieldCodec.ForMessage(18, global::PBBSCommon.FrameInputCmd.Parser);
    private readonly pbc::RepeatedField<global::PBBSCommon.FrameInputCmd> inputCmds_ = new pbc::RepeatedField<global::PBBSCommon.FrameInputCmd>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::PBBSCommon.FrameInputCmd> InputCmds {
      get { return inputCmds_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as FrameInput);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(FrameInput other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Tick != other.Tick) return false;
      if(!inputCmds_.Equals(other.inputCmds_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Tick != 0) hash ^= Tick.GetHashCode();
      hash ^= inputCmds_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Tick != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Tick);
      }
      inputCmds_.WriteTo(output, _repeated_inputCmds_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Tick != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Tick);
      }
      size += inputCmds_.CalculateSize(_repeated_inputCmds_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(FrameInput other) {
      if (other == null) {
        return;
      }
      if (other.Tick != 0) {
        Tick = other.Tick;
      }
      inputCmds_.Add(other.inputCmds_);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            Tick = input.ReadInt32();
            break;
          }
          case 18: {
            inputCmds_.AddEntriesFrom(input, _repeated_inputCmds_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
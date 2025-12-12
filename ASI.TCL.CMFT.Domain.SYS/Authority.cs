namespace ASI.TCL.CMFT.Domain.SYS
{
    public enum AuthPremission
    {
        PAFunc,
        PASetting,
        DMDFunc,
        DMDSetting,
        TetraFunc,
        TetraSetting,
        SYSSetting,

        OTCSFunc,
        AlarmSetting,
       
    }
    public class Authority : Value<Authority>
    {
       
        public static readonly List<Authority> AuthorityList =
            Enum.GetNames(typeof(AuthPremission))
                .Select(name => new Authority(name))
                .ToList();


        public string Code { get; }

        
          

        public Authority(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("權限代碼不可為空", nameof(code));

            Code = code;
        }

        public static implicit operator string(Authority self) => self.Code;
        public static implicit operator Authority(string code) => new Authority(code);

        public override string ToString() => Code;
    }

}
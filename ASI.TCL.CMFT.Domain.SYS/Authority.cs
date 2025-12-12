namespace ASI.TCL.CMFT.Domain.SYS
{
    public class Authority : Value<Authority>
    {
        public string Code { get; }

        public static string AutStringhList
            => "PAFunc,PASetting,DMDFunc,DMDSetting,TetraFunc,TetraSetting,OTCSFunc,AlarmSetting,UserSetting";

        public static List<Authority> AuthList
            => AutStringhList
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(code => new Authority(code.Trim()))
                .ToList();

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
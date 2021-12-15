namespace Incidences.Business
{
    public interface IPasswordRecoveryBz
    {
        public bool RecoverPassword(string username);
        public bool CheckRecoverCode(string code, string username);
    }
}

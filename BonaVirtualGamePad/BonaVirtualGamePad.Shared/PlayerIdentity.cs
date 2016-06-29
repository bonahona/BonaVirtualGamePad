using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    public class PlayerIdentity
    {
        // A 128bit pattern to identify a player (16 x 8 = 128)
        public const int PATTERN_LENGTH = 16;

        public static bool operator ==(PlayerIdentity a, PlayerIdentity b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(PlayerIdentity a, PlayerIdentity b)
        {
            return !a.Equals(b);
        }

        public static PlayerIdentity GenerateNew()
        {
            PlayerIdentity result = new PlayerIdentity();
            return result;
        }

        private byte[] m_identity;

        // Stored Hexadecimal copy of the identity byte pattern
        private String m_identityString;
        // Stored hashcode so it does not have to be recalculated every time it's used
        private int m_hashCode;

        // Returns a copy of the byte pattern
        public byte[] Identity
        {
            get { return Copy(); }
        }

        public PlayerIdentity()
        {
            m_identity = new byte[PATTERN_LENGTH];

            RegenerateIdentityHash();
        }

        public PlayerIdentity(byte[] data)
        {
            if(data.Length != PATTERN_LENGTH){
                throw new BonaVirtualGamePadException(String.Format("PlayerIdentity intial byte count is not {0}", PATTERN_LENGTH));
            }

            for(int i = 0; i < PATTERN_LENGTH; i++){
                m_identity[i] = data[i];
            }

            RegenerateIdentityHash();
        }

        public override string ToString()
        {
            return m_identityString;
        }

        public override int GetHashCode()
        {
            return m_hashCode;
        }

        public override bool Equals(object obj)
        {
            if(obj is PlayerIdentity){
                PlayerIdentity other = (PlayerIdentity)obj;
                return other.Equals(m_identity);
            }
            else{
                return false;
            }
        }

        public bool Equals(byte[] other)
        {
            for(int i = 0; i < other.Length; i++){
                if(m_identity[i] != other[i]){
                    return false;
                }
            }
            return true;
        }

        private byte[] Copy()
        {
            byte[] result = new byte[m_identity.Length];
            for(int i = 0; i < result.Length; i ++){
                result[i] = m_identity[i];
            }

            return result;
        }

        private void RegenerateIdentityHash()
        {
            m_identityString = "";
            m_hashCode = 0;
        }
    }
}

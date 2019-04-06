using System;
using UnityEngine;
using UniRx;

namespace Player
{
    public class PlayerMovementController
    {
        private FloatReactiveProperty m_Movement;
        private MovementParameters m_Params;
        private Positioning m_Positioning;
        private Info m_Player;

        private float m_CurrentMove = 0f, m_DampSpeed = 3f;

        public PlayerMovementController SetPlayer(Info info)
        {
            m_Player = info;
            m_Positioning = info.Positioning;
            return this;
        }

        public PlayerMovementController SetInput(IUserInput input)
        {
            m_Movement = input.CharacterMoving;
            return this;
        }

        public PlayerMovementController SetParameters(MovementParameters param)
        {
            m_Params = param;
            return this;
        }

        public void Reset()
        {
            m_CurrentMove = 0f;
        }

        public void Update(float deltaTime)
        {
            if (!m_Player.Health.IsAlive || m_Movement == null) return;
            float move = m_Movement.Value;
            if (move == 0f && m_CurrentMove == 0f) return;

            if (m_CurrentMove != move)
            {
                float sign = (move - m_CurrentMove) / Mathf.Abs(move - m_CurrentMove);
                float newMove = m_CurrentMove + deltaTime * sign * m_DampSpeed;
                m_CurrentMove = Mathf.Clamp(newMove, Mathf.Min(move, m_CurrentMove), Mathf.Max(move, m_CurrentMove));
            }

            Vector3 pos = m_Positioning.Position.Value;
            pos.z += deltaTime * m_CurrentMove * m_Params.Speed * -1f;
            pos.z = Mathf.Clamp(pos.z, m_Params.LeftBorder, m_Params.RightBorder);
            m_Positioning.Position.Value = pos;
            m_Positioning.Rotation.Value = Quaternion.Lerp(m_Params.LeftRotation, m_Params.RightRotation, (m_CurrentMove + 1f) * 0.5f);
        }
    }
    
    public struct MovementParameters
    {
        public float LeftBorder, RightBorder, Speed;
        public Quaternion LeftRotation, RightRotation;

        public MovementParameters(MovementParametersMono param, float leftBorder, float rightBorder)
        {
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
            Speed = param.Speed;
            LeftRotation = Quaternion.Euler(param.LeftRotation, 0f, 0f);
            RightRotation = Quaternion.Euler(param.RightRotation, 0f, 0f);
        }
    }

    [Serializable]
    public struct MovementParametersMono
    {
        public float Speed, LeftRotation, RightRotation;
    }
}
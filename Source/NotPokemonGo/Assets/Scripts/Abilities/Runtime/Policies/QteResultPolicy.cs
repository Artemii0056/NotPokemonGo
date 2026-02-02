using System;
using Abilities.Configs;
using Abilities.Signals;
using QteSystem;
using QteSystem.TestQTE;
using UnityEngine;

namespace Abilities.Runtime.Policies
{
    public class QteResultPolicy : AbilityPolicyBase
    {
        private readonly IQteService _qteService;

        private AbilityContext _context;
        private IQteSession _qteSession;

        private IDisposable _gateToken;
        private bool _startedThisPhase;

        public QteResultPolicy(IQteService qteService)
        {
            _qteService = qteService;
        }

        public override void OnPhaseStart(AbilityContext ctx, AbilityPhase phase)
        {
            _context = ctx;
            _startedThisPhase = false;

            _qteSession = null;

            _gateToken = null;

            if (phase.QteType != QteType.Unknown)
            {
                _gateToken = ctx.AnimatorTrigger.PhaseService.AcquireFinishToken("QTE");
                Debug.Log("[QTE] Gate token ACQUIRED");
            }
        }

        public override void OnSignal(AbilityContext ctx, PhaseSignal signal)
        {
            if (_startedThisPhase)
                return;

            if (ctx.CurrentPhase.QteType == QteType.Unknown)
                return;

            _startedThisPhase = true;

            _qteSession = _qteService.StartSession(
                ctx.CurrentPhase.QteType,
                ctx.Source,
                1.2f
            );

            _qteSession.Completed += OnCompleted;
        }

        private void OnCompleted(QteResult result)
        {
            Debug.Log($"[QTE] Completed={result} disposingToken={_gateToken!=null}");
            Debug.Log($"[QTE] Completed with {result}");

            _qteSession.Completed -= OnCompleted;
            _context.QteResult = result;

            _gateToken.Dispose();
            _gateToken = null;

            _context.AnimatorTrigger.PhaseService.RequestFinishCheck();
        }

        public override void OnAbilityStop(AbilityContext ctx)
        {
            _gateToken?.Dispose();
            _gateToken = null;

            _qteSession = null;
            _context = null;
        }
    }
}
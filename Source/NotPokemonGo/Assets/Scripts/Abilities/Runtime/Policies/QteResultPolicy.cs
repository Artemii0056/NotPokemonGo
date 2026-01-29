using Abilities.Signals;
using QTESystem;
using QTESystem.TestQTE;
using UnityEngine;

namespace Abilities.Runtime.Policies
{
    public class QteResultPolicy : AbilityPolicyBase
    {
        private readonly IQteService _qteService;
        private AbilityContext _context;
        private IQteSession _qteSession;

        public QteResultPolicy(IQteService qteService) => 
            _qteService = qteService;

        public override void OnSignal(AbilityContext ctx, PhaseSignal signal)
        {
            if (ctx.CurrentPhase.QteType == QteType.Unknown)
            {
                _context.Animator?.FlagSignal((int)PhaseSignal.Finish);
                return;
            }
            
            _context = ctx;

            _qteSession= _qteService.StartSession(_context.CurrentPhase.QteType, ctx.Source,2.1f);

            _qteSession.Completed += Cancel;
        }

        private void Cancel(QteResult qteResult)
        {
            Debug.Log(qteResult + " Cancel");
            
            _qteSession.Completed -= Cancel;
            
            _context.QteResult = qteResult;
            _context.Animator?.FlagSignal((int)PhaseSignal.Finish);
        }
    }
}
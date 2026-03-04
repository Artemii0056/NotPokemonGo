FINAL PATCH (2026-02-24)

Состав:
- AbilityService: всегда получает handler из IAbilityHandlerFactory (без switch).
- AbilityHandlerRouterFactory: pipeline-first, fallback legacy без throw + логи PIPELINE/LEGACY.
- FireBall pipeline (AbilityPipelineFactory): 3 shots cap, QTE UniTask (через StartSession), задержка выстрелов зависит от QTE.
- ComposeVolleyStep: maxShots cap.
- LaunchPreparedMoversOnAttackStep: таймаут ожидания Attack1 + cancel-safe gateToken (no double-dispose) + dynamic delay.
- AbilityExecutionContext: хранит QteResult.
- UniTask extensions: IQteSession.WaitResultAsync + IQteService.RunAsync (extension) — интерфейс не ломаем.
- DI patch txt: как зарегистрировать router factory.

Установка:
1) Распакуй архив в корень Unity проекта (чтобы попало в Assets/Scripts) и согласись на замену файлов.
2) Примени DI правку (см. RegisterGlobalServices_FINAL_PATCH.txt).
3) Запусти бой. В консоли будет:
   - [AbilityRouter] X: PIPELINE / LEGACY fallback

Дальше по этапам:
- Этап 2: BaseAttack -> pipeline
- Этап 3: DroneBaseAttack -> pipeline
- Этап 4: убрать корутины из Continue2 (UniTask.Delay + cancellation)

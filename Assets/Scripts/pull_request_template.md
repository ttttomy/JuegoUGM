## Objetivo
Refactorización **variable por variable** en `PlayerMove` para mejorar legibilidad y coherencia sin cambiar comportamiento.

## Tabla de cambios
| Antes               | Después     | Motivo                                  | Riesgo Inspector |
|---------------------|-------------|------------------------------------------|------------------|
| myCaracterController| controller  | nombre claro, typo fix                   | Mitigado con `FormerlySerializedAs` |
| camAnim            | animator    | desacoplar de “cámara”                   | Mitigado         |
| playerSpeed        | moveSpeed   | consistencia semántica                   | Mitigado         |
| momentumDamping    | inputDamping| precisión (amortigua la entrada)         | N/A              |
| myGravity          | gravity     | convención y valor físico                | N/A              |
| inputVector        | moveInput   | intención de uso                         | N/A              |
| movementVector     | motion      | nomenclatura concisa                     | N/A              |

## Criterios de aceptación
- Compila y corre igual que antes.
- No se pierden referencias del Inspector (gracias a `FormerlySerializedAs`).
- Velocidad en diagonal no cambia vs. antes.
- Animator sigue recibiendo `isWalking`.

## Cómo revisar (paso a paso)
1. Abrir pestaña **Files changed**.
2. Ir a cada diff de variable y leer el **comentario docente** adjunto.
3. Ejecutar escena y validar comportamiento con el checklist.

## Categorías de cambios generales

- **feat**: Una nueva feature (funcionalidad)
- **fix**: Corrección de errores
- **refactor**: Cambios en código que no corrigen o añaden una función
- **perf**: Cambio en código que mejora el rendimiento
- **style**: Cambios que no afectan al funcionamiento (espacios en blanco, formateo de código, etc.)
- **docs**: Cambios en documentación
- **chore**: Tareas rutinarias (actualización de dependencias, etc)
- **revert**: Revertir un commit
- **merge**: Fusiones de ramas (merges) o resolución de conflictos
- **other**: Cambios que no entran en los anteriores puntos.

## Categorías de cambios específicas

- **gfx**: Cambios en gráficos
- **audio**: Cambios en audio
- **ui**: Cambios en UI
- **anim**: Cambios en animaciones
- **input**: Cambios en Player Input
- **level**: Cambios en diseño de nivel o entornos
- **phys**: Cambios en funcionamiento de físicas
- **ai**: Cambios en funcionamiento de Inteligencia Artificial

### Referencia

Esta indica a que tarjeta de Trello corresponden los cambios realizados en el proyecto. Esta se representa de la siguiente forma:

```scss
[Ref: IDTarjeta]
```

Puede existir el caso en que un commit no cuente con una referencia, principalmente en las casos de **_revert_**.
‎
‎

## Ejemplos

- **feat:** _Nuevo comportamiento de IA enemiga_ [Ref: ENMY_AI]
- **fix:** _Solución a error en detección de colisiones de jugador_ [Ref: COLL_DET]
- **refactor:** _Optimización en el pipeline de rendereo_ [Ref: RENDER_OPT]
- **perf:** _Mejora en framerate por ajuste de texturas_ [Ref: FRMRT_TEX]
- **style:** _Código de Player formateado_ [Ref: PLAYER]
- **docs:** _Actualización de README con info de configuración_ [Ref: README_CFG]
- **chore:** _Actualización de dependencias_ [Ref: DEP_UPD]
- **revert**: _revertir "feat: nuevo comportamiento de IA enemiga"_
- **merge:** _Fusión de la rama feature/animaciones con main_ [Ref: ANIM_MERGE]
- **gfx:** _Nuevas texturas de terreno implementadas_ [Ref: TERRAIN_TEX]
- **audio:** _Ajuste de comportamiento de música de fondo_ [Ref: BGM_ADJ]
- **ui:** _Rediseño de layout del Menu Principal_ [Ref: MENU_UI]
- **anim:** _Animación de caminar de NPC implementada_ [Ref: NPC_WALK]
- **input:** _Cambios en controles de player (movimiento)_ [Ref: PLAYER_CTRL]
- **level:** _Nivel de bosque añadido_ [Ref: FOREST_LVL]
- **phys:** _Ajuste en gravedad para que el salto sea más realista_ [Ref: GRAV_JUMP]
- **ai**: _Mejora en la toma de decisiones del enemigo_ [Ref: ENMY_AI_DEC]

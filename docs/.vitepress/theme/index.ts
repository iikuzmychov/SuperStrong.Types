import DefaultTheme from 'vitepress/theme'
import type { Theme } from 'vitepress'
import ImplementDefineQuickAction from './components/ImplementDefineQuickAction.vue'
import ImplementDefineExplicitlyQuickAction from './components/ImplementDefineExplicitlyQuickAction.vue'
import OverrideToStringQuickAction from './components/OverrideToStringQuickAction.vue'
import OverrideEqualsAndGetHashCodeQuickAction from './components/OverrideEqualsAndGetHashCodeQuickAction.vue'
import './custom.css'

export default {
  extends: DefaultTheme,
  enhanceApp({ app }) {
    app.component('ImplementDefineQuickAction', ImplementDefineQuickAction)
    app.component('ImplementDefineExplicitlyQuickAction', ImplementDefineExplicitlyQuickAction)
    app.component('OverrideToStringQuickAction', OverrideToStringQuickAction)
    app.component('OverrideEqualsAndGetHashCodeQuickAction', OverrideEqualsAndGetHashCodeQuickAction)
  },
} satisfies Theme

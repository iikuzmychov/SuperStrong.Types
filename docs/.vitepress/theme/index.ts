import DefaultTheme from 'vitepress/theme'
import type { Theme } from 'vitepress'
import ImplementDefineQuickAction from './components/ImplementDefineQuickAction.vue'
import ImplementDefineExplicitlyQuickAction from './components/ImplementDefineExplicitlyQuickAction.vue'
import GenerateToStringQuickAction from './components/GenerateToStringQuickAction.vue'
import GenerateEqualsAndGetHashCodeQuickAction from './components/GenerateEqualsAndGetHashCodeQuickAction.vue'
import './custom.css'

export default {
  extends: DefaultTheme,
  enhanceApp({ app }) {
    app.component('ImplementDefineQuickAction', ImplementDefineQuickAction)
    app.component('ImplementDefineExplicitlyQuickAction', ImplementDefineExplicitlyQuickAction)
    app.component('GenerateToStringQuickAction', GenerateToStringQuickAction)
    app.component('GenerateEqualsAndGetHashCodeQuickAction', GenerateEqualsAndGetHashCodeQuickAction)
  },
} satisfies Theme

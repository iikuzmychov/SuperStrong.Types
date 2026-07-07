import DefaultTheme from 'vitepress/theme'
import type { Theme } from 'vitepress'
import AddDefineQuickAction from './components/AddDefineQuickAction.vue'
import OverrideToStringQuickAction from './components/OverrideToStringQuickAction.vue'
import OverrideEqualsAndGetHashCodeQuickAction from './components/OverrideEqualsAndGetHashCodeQuickAction.vue'
import './custom.css'

export default {
  extends: DefaultTheme,
  enhanceApp({ app }) {
    app.component('AddDefineQuickAction', AddDefineQuickAction)
    app.component('OverrideToStringQuickAction', OverrideToStringQuickAction)
    app.component('OverrideEqualsAndGetHashCodeQuickAction', OverrideEqualsAndGetHashCodeQuickAction)
  },
} satisfies Theme

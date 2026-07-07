<script setup lang="ts">
import QuickActionMockup from './QuickActionMockup.vue'
</script>

<template>
  <QuickActionMockup
    primitive="string"
    class-name="Password"
    :menu="[
      { label: `Generate constructor 'Password()'` },
      { label: 'Extract base class...' },
      { label: 'Move type to Password.cs' },
      { label: 'Move to namespace...' },
      { label: 'Generate overrides...' },
      { label: 'Add Define' },
      { label: 'Override Equals(T) and GetHashCode()' },
      { label: 'Override ToString()', selected: true },
      { label: `Add 'DebuggerDisplay' attribute` },
    ]"
  >
    <div class="line indent-4 added"><span class="plus">+</span><span class="keyword">public override string</span> <span class="method">ToString</span>() => _value.<span class="method">ToString</span>();</div>
  </QuickActionMockup>
</template>

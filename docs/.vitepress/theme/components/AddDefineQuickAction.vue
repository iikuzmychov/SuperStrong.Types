<script setup lang="ts">
import QuickActionMockup from './QuickActionMockup.vue'
</script>

<template>
  <QuickActionMockup
    primitive="int"
    class-name="Age"
    :menu="[
      { label: `Generate constructor 'Age()'` },
      { label: 'Extract base class...' },
      { label: 'Move type to Age.cs' },
      { label: 'Move to namespace...' },
      { label: 'Generate overrides...' },
      { label: 'Add Define()', selected: true },
      { label: 'Override Equals(T) and GetHashCode()' },
      { label: 'Override ToString()' },
      { label: `Add 'DebuggerDisplay' attribute` },
    ]"
  >
    <div class="line indent-4 added"><span class="plus">+</span><span class="keyword">public static partial</span> <span class="type">StrongTypeDefinition</span>&lt;<span class="keyword">int</span>&gt; <span class="method">Define</span>() => <span class="type">StrongType</span>.<span class="method">Define</span>&lt;<span class="keyword">int</span>&gt;();</div>
  </QuickActionMockup>
</template>

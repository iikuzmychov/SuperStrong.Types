<script setup lang="ts">
import QuickActionMockup from './QuickActionMockup.vue'
</script>

<template>
  <QuickActionMockup
    primitive="string"
    class-name="Currency"
    :menu="[
      { label: 'Generate constructor from members...' },
      { label: `Generate constructor 'Currency()'` },
      { label: 'Extract interface...' },
      { label: 'Extract base class...' },
      { label: 'Implement Define()' },
      { label: 'Implement Define() explicitly' },
      { label: 'Override Equals(T) and GetHashCode()', selected: true },
      { label: 'Override ToString()' },
    ]"
  >
    <div class="line indent-4 added"><span class="plus">+</span><span class="keyword">public bool</span> <span class="method">Equals</span>(<span class="type">Currency</span>? other) => other <span class="keyword">is not null</span> &amp;&amp; _value.<span class="method">Equals</span>(other._value);</div>
    <div class="line indent-4 added"><span class="plus">+</span><span class="keyword">public override int</span> <span class="method">GetHashCode</span>() => _value.<span class="method">GetHashCode</span>();</div>
  </QuickActionMockup>
</template>

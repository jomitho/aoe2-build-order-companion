export type BuildOrderSummary = {
  id: number;
  name: string;
  civilization: string;
  strategyType: string;
  difficulty: string;
  description?: string;
  stepCount: number;
};

export type BuildOrderStep = {
  id: number;
  buildOrderId: number;
  stepNumber: number;
  population?: number;
  age: string;
  instruction: string;
  resourceFocus?: string;
  notes?: string;
};

export type BuildOrderDetail = {
  id: number;
  name: string;
  civilization: string;
  strategyType: string;
  difficulty: string;
  description?: string;
  steps: BuildOrderStep[];
};

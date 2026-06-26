import type { BuildOrderDetail, BuildOrderSummary } from "../types/buildOrder";

const API_BASE_URL = "http://localhost:5198";

export type BuildOrderStepInput = {
  stepNumber: number;
  population?: number;
  age: string;
  instruction: string;
  resourceFocus?: string;
  notes?: string;
};

export type CreateBuildOrderInput = {
  name: string;
  civilization: string;
  strategyType: string;
  difficulty: string;
  description?: string;
  steps: BuildOrderStepInput[];
};

export async function getBuildOrders(): Promise<BuildOrderSummary[]> {
  const response = await fetch(`${API_BASE_URL}/api/buildorders`);

  if (!response.ok) {
    throw new Error("Failed to fetch build orders");
  }

  return response.json();
}

export async function getBuildOrderById(id: number): Promise<BuildOrderDetail> {
  const response = await fetch(`${API_BASE_URL}/api/buildorders/${id}`);

  if (!response.ok) {
    throw new Error("Failed to fetch build order");
  }

  return response.json();
}

export async function createBuildOrder(
  input: CreateBuildOrderInput
): Promise<BuildOrderDetail> {
  const response = await fetch(`${API_BASE_URL}/api/buildorders`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(input),
  });

  if (!response.ok) {
    throw new Error("Failed to create build order");
  }

  return response.json();
}

export async function deleteBuildOrder(id: number): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/api/buildorders/${id}`, {
    method: "DELETE",
  });

  if (!response.ok) {
    throw new Error("Failed to delete build order");
  }
}

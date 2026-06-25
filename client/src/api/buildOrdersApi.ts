import type { BuildOrderSummary } from "../types/buildOrder";

const API_BASE_URL = "http://localhost:5198";

export async function getBuildOrders(): Promise<BuildOrderSummary[]> {
  const response = await fetch(`${API_BASE_URL}/api/buildorders`);

  if (!response.ok) {
    throw new Error("Failed to fetch build orders");
  }

  return response.json();
}
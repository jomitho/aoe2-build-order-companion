import { useState } from "react";
import type { BuildOrderDetail } from "../types/buildOrder";
import type { UpdateBuildOrderInput } from "../api/buildOrdersApi";

type EditBuildOrderFormProps = {
  buildOrder: BuildOrderDetail;
  onUpdate: (input: UpdateBuildOrderInput) => Promise<void>;
  onCancel: () => void;
  isUpdating: boolean;
};

export function EditBuildOrderForm({
  buildOrder,
  onUpdate,
  onCancel,
  isUpdating,
}: EditBuildOrderFormProps) {
  const [name, setName] = useState(buildOrder.name);
  const [civilization, setCivilization] = useState(buildOrder.civilization);
  const [strategyType, setStrategyType] = useState(buildOrder.strategyType);
  const [difficulty, setDifficulty] = useState(buildOrder.difficulty);
  const [description, setDescription] = useState(buildOrder.description ?? "");

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const input: UpdateBuildOrderInput = {
      name,
      civilization,
      strategyType,
      difficulty,
      description,
      steps: buildOrder.steps.map((step) => ({
        stepNumber: step.stepNumber,
        population: step.population,
        age: step.age,
        instruction: step.instruction,
        resourceFocus: step.resourceFocus,
        notes: step.notes,
      })),
    };

    await onUpdate(input);
  }

  return (
    <form className="editForm" onSubmit={handleSubmit}>
      <div className="formHeader">
        <div>
          <p className="eyebrow dark">Edit</p>
          <h2>Edit build order</h2>
        </div>
      </div>

      <label>
        Name
        <input
          value={name}
          onChange={(event) => setName(event.target.value)}
          required
        />
      </label>

      <div className="formGrid">
        <label>
          Civilization
          <input
            value={civilization}
            onChange={(event) => setCivilization(event.target.value)}
            required
          />
        </label>

        <label>
          Strategy
          <input
            value={strategyType}
            onChange={(event) => setStrategyType(event.target.value)}
            required
          />
        </label>

        <label>
          Difficulty
          <select
            value={difficulty}
            onChange={(event) => setDifficulty(event.target.value)}
            required
          >
            <option>Beginner</option>
            <option>Intermediate</option>
            <option>Advanced</option>
          </select>
        </label>
      </div>

      <label>
        Description
        <textarea
          value={description}
          onChange={(event) => setDescription(event.target.value)}
          rows={3}
        />
      </label>

      <div className="formActions">
        <button className="primaryButton" type="submit" disabled={isUpdating}>
          {isUpdating ? "Saving..." : "Save changes"}
        </button>

        <button className="secondaryButton" type="button" onClick={onCancel}>
          Cancel
        </button>
      </div>
    </form>
  );
}

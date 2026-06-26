import { useState } from "react";
import type { CreateBuildOrderInput } from "../api/buildOrdersApi";

type CreateBuildOrderFormProps = {
  onCreate: (input: CreateBuildOrderInput) => Promise<void>;
  isCreating: boolean;
};

export function CreateBuildOrderForm({
  onCreate,
  isCreating,
}: CreateBuildOrderFormProps) {
  const [name, setName] = useState("");
  const [civilization, setCivilization] = useState("Generic");
  const [strategyType, setStrategyType] = useState("");
  const [difficulty, setDifficulty] = useState("Beginner");
  const [description, setDescription] = useState("");

  const [stepOneInstruction, setStepOneInstruction] = useState("");
  const [stepTwoInstruction, setStepTwoInstruction] = useState("");
  const [stepThreeInstruction, setStepThreeInstruction] = useState("");

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const input: CreateBuildOrderInput = {
      name,
      civilization,
      strategyType,
      difficulty,
      description,
      steps: [
        {
          stepNumber: 1,
          population: 6,
          age: "Dark Age",
          instruction: stepOneInstruction,
          resourceFocus: "Food",
        },
        {
          stepNumber: 2,
          population: 10,
          age: "Dark Age",
          instruction: stepTwoInstruction,
          resourceFocus: "Wood",
        },
        {
          stepNumber: 3,
          population: 16,
          age: "Dark Age",
          instruction: stepThreeInstruction,
          resourceFocus: "Food",
        },
      ],
    };

    await onCreate(input);

    setName("");
    setCivilization("Generic");
    setStrategyType("");
    setDifficulty("Beginner");
    setDescription("");
    setStepOneInstruction("");
    setStepTwoInstruction("");
    setStepThreeInstruction("");
  }

  return (
    <form className="createForm" onSubmit={handleSubmit}>
      <div className="formHeader">
        <div>
          <p className="eyebrow dark">Create</p>
          <h2>New build order</h2>
        </div>
      </div>

      <label>
        Name
        <input
          value={name}
          onChange={(event) => setName(event.target.value)}
          placeholder="Example: 20 Pop Scouts"
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
            placeholder="Scouts, Archers, Boom..."
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
          placeholder="Short description of the build order"
          rows={3}
        />
      </label>

      <div className="stepInputs">
        <h3>Opening steps</h3>

        <label>
          Step 1 — Pop 6 / Food
          <input
            value={stepOneInstruction}
            onChange={(event) => setStepOneInstruction(event.target.value)}
            placeholder="Send starting villagers to sheep."
            required
          />
        </label>

        <label>
          Step 2 — Pop 10 / Wood
          <input
            value={stepTwoInstruction}
            onChange={(event) => setStepTwoInstruction(event.target.value)}
            placeholder="Send villagers to wood and build a lumber camp."
            required
          />
        </label>

        <label>
          Step 3 — Pop 16 / Food
          <input
            value={stepThreeInstruction}
            onChange={(event) => setStepThreeInstruction(event.target.value)}
            placeholder="Send villagers to berries and build a mill."
            required
          />
        </label>
      </div>

      <button className="primaryButton" type="submit" disabled={isCreating}>
        {isCreating ? "Creating..." : "Create build order"}
      </button>
    </form>
  );
}

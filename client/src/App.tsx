import { useEffect, useState } from 'react';
import "./App.css";

import { 
  createBuildOrder, 
  deleteBuildOrder,
  getBuildOrderById, 
  getBuildOrders,
  type CreateBuildOrderInput, 
} from "./api/buildOrdersApi";

import type { BuildOrderDetail, BuildOrderSummary } from "./types/buildOrder";

function App() {
  const [buildOrders, setBuildOrders] = useState<BuildOrderSummary[]>([]);
  const [selectedBuildOrder, setSelectedBuildOrder] =
    useState<BuildOrderDetail | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isLoadingDetail, setIsLoadingDetail] = useState(false);
  const [isCreating, setIsCreating] = useState(false);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  useEffect(() => {
    async function loadBuildOrders() {
      try {
        const data = await getBuildOrders();
        setBuildOrders(data);

        if (data.length > 0) {
          await loadBuildOrderDetail(data[0].id);
        }
      } catch {
        setErrorMessage("Could not load build orders. Is the API running?");
      } finally {
        setIsLoading(false);
      }
    }

    loadBuildOrders();
  }, []);

  async function loadBuildOrderDetail(id: number) {
    setIsLoadingDetail(true);
    setErrorMessage(null);

    try {
      const detail = await getBuildOrderById(id);
      setSelectedBuildOrder(detail);
    } catch {
      setErrorMessage("Could not load build order details.");
    } finally {
      setIsLoadingDetail(false);
    }
  }

  async function handleCreateSampleBuildOrder() {
    setIsCreating(true);
    setErrorMessage(null);

    const input: CreateBuildOrderInput = {
      name: "Men-at-Arms into Archers",
      civilization: "Generic",
      strategyType: "Infantry/Archers",
      difficulty: "Intermediate",
      description:
        "A Feudal Age opening that applies early pressure with militia before transitioning into archers.",
      steps: [
        {
          stepNumber: 1,
          population: 6,
          age: "Dark Age",
          instruction: "Send starting villagers to sheep.",
          resourceFocus: "Food",
          notes: "Keep the town center producing villagers.",
        },
        {
          stepNumber: 2,
          population: 10,
          age: "Dark Age",
          instruction: "Send villagers to wood and build a lumber camp.",
          resourceFocus: "Wood",
        },
        {
          stepNumber: 3,
          population: 14,
          age: "Dark Age",
          instruction: "Send villagers to berries and build a mill.",
          resourceFocus: "Food",
        },
        {
          stepNumber: 4,
          population: 18,
          age: "Dark Age",
          instruction: "Send villagers to gold and prepare militia production.",
          resourceFocus: "Gold",
        },
        {
          stepNumber: 5,
          population: 21,
          age: "Dark Age",
          instruction: "Click up to Feudal Age and upgrade militia to Men-at-Arms.",
          resourceFocus: "Food/Gold",
        },
        {
          stepNumber: 6,
          population: 22,
          age: "Feudal Age",
          instruction: "Build archery ranges and transition into archer production.",
          resourceFocus: "Wood/Gold",
        },
      ],
    };

    try {
      const createdBuildOrder = await createBuildOrder(input);
      const updatedBuildOrders = await getBuildOrders();

      setBuildOrders(updatedBuildOrders);
      setSelectedBuildOrder(createdBuildOrder);
    } catch {
      setErrorMessage("Could not create build order.");
    } finally {
      setIsCreating(false);
    }
  }

  async function handleDeleteSelectedBuildOrder() {
    if (!selectedBuildOrder) {
      return;
    }

    const shouldDelete = window.confirm(
      `Delete "${selectedBuildOrder.name}"? This cannot be undone.`
    );

    if (!shouldDelete) {
      return;
    }

    try {
      await deleteBuildOrder(selectedBuildOrder.id);

      const updatedBuildOrders = await getBuildOrders();
      setBuildOrders(updatedBuildOrders);

      if (updatedBuildOrders.length > 0) {
        await loadBuildOrderDetail(updatedBuildOrders[0].id);
      } else {
        setSelectedBuildOrder(null);
      }
    } catch {
      setErrorMessage("Could not delete build order.");
    }
  }

  return (

    <main className="app">
      <section className="hero">
        <p className="eyebrow">Age of Empires II</p>
        <h1>Build Order Companion</h1>
        <p className="heroText">
          Create, manage and follow build orders for Age of Empires II.
        </p>
      </section>

      <section className="content">

        <div className="sectionHeader">
          <div>
            <h2>Build orders</h2>
            <span>{buildOrders.length} available</span>
          </div>

          <button
            className="primaryButton"
            onClick={handleCreateSampleBuildOrder}
            disabled={isCreating}
          >
            {isCreating ? "Creating..." : "Add sample build order"}
          </button>
        </div>

        {isLoading && <p>Loading build orders...</p>}

        {errorMessage && <p className="error">{errorMessage}</p>}

        {!isLoading && (
          <div className="layout">
            <div className="buildOrderList">
              {buildOrders.map((buildOrder) => (
                <button
                  key={buildOrder.id}
                  className={
                    selectedBuildOrder?.id === buildOrder.id
                      ? "buildOrderCard selected"
                      : "buildOrderCard"
                  }
                  onClick={() => loadBuildOrderDetail(buildOrder.id)}
                >
                  <div className="cardHeader">
                    <h3>{buildOrder.name}</h3>
                    <span className="difficulty">{buildOrder.difficulty}</span>
                  </div>

                  <p className="description">{buildOrder.description}</p>

                  <dl className="metadata">
                    <div>
                      <dt>Civilization</dt>
                      <dd>{buildOrder.civilization}</dd>
                    </div>
                    <div>
                      <dt>Strategy</dt>
                      <dd>{buildOrder.strategyType}</dd>
                    </div>
                    <div>
                      <dt>Steps</dt>
                      <dd>{buildOrder.stepCount}</dd>
                    </div>
                  </dl>
                </button>
              ))}
            </div>

            <section className="detailPanel">
              {isLoadingDetail && <p>Loading details...</p>}

              {!isLoadingDetail && selectedBuildOrder && (
                <>

<div className="detailHeader">
  <div className="detailTitleRow">
    <div>
      <p className="eyebrow dark">{selectedBuildOrder.strategyType}</p>
      <h2>{selectedBuildOrder.name}</h2>
    </div>

    <button
      className="dangerButton"
      onClick={handleDeleteSelectedBuildOrder}
    >
      Delete
    </button>
  </div>

  <p>{selectedBuildOrder.description}</p>
</div>

                  <div className="stepTimeline">
                    {selectedBuildOrder.steps.map((step) => (
                      <article key={step.id} className="stepItem">
                        <div className="stepNumber">{step.stepNumber}</div>

                        <div className="stepContent">
                          <div className="stepMeta">
                            <span>{step.age}</span>
                            {step.population && <span>Pop {step.population}</span>}
                            {step.resourceFocus && (
                              <span>{step.resourceFocus}</span>
                            )}
                          </div>

                          <p>{step.instruction}</p>

                          {step.notes && <small>{step.notes}</small>}
                        </div>
                      </article>
                    ))}
                  </div>
                </>
              )}
            </section>
          </div>
        )}
      </section>
    </main>
  );
}

export default App

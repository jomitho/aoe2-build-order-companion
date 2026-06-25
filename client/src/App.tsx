import { useEffect, useState } from 'react';
import "./App.css";
import { getBuildOrderById, getBuildOrders } from "./api/buildOrdersApi";
import type { BuildOrderDetail, BuildOrderSummary } from "./types/buildOrder";

function App() {
  const [buildOrders, setBuildOrders] = useState<BuildOrderSummary[]>([]);
  const [selectedBuildOrder, setSelectedBuildOrder] =
    useState<BuildOrderDetail | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isLoadingDetail, setIsLoadingDetail] = useState(false);
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
          <h2>Build orders</h2>
          <span>{buildOrders.length} available</span>
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
                    <p className="eyebrow dark">{selectedBuildOrder.strategyType}</p>
                    <h2>{selectedBuildOrder.name}</h2>
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
